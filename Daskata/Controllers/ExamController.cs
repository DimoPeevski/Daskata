﻿using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Daskata.Core.Shared.Methods;

namespace Daskata.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly ILogger<ExamController> _logger;
        private readonly UserManager<UserProfile> _userManager;
        private readonly DaskataDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExamController(ILogger<ExamController> logger,
                              IHttpContextAccessor httpContextAccessor,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("All", "Exam");
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public IActionResult Create()
        {
            var subjects = Enum.GetValues(typeof(SubjectCategory));
            var subjectList = new List<string>();

            foreach (var subject in subjects)
            {
                subjectList.Add(subject.ToString());
            }

            return View(subjectList);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Create/Grade")]
        [HttpPost]
        public IActionResult Grade([FromForm] string subject)
        {
            var grades = Enum.GetValues(typeof(GradeCategory));
            var gradeList = new List<string>();

            foreach (var grade in grades)
            {
                gradeList.Add(grade.ToString());
            }

            HttpContext.Session.SetString("Subject", subject);
            return View(gradeList);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Create/Grade/Details")]
        [HttpPost]
        public IActionResult Details([FromForm] string grade)
        {
            var model = new FullExamViewModel()
            {
                Title = string.Empty,
                Description = string.Empty,
                Duration = 30,
                TotalPoints = 60,
                IsPublic = true,
                StudySubject = 0,
                StudentGrade = 0,
            };

            HttpContext.Session.SetString("Grade", grade);
            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Create/Grade/Details/Publish")]
        [HttpPost]
        public async Task<IActionResult> Publish(FullExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Заглавието е задълително.");
                return View(model);
            }

            TimeSpan duration = TimeSpan.FromMinutes(model.Duration);

            var studySubject = (SubjectCategory)Enum.Parse(typeof(SubjectCategory), HttpContext.Session.GetString("Subject"));
            var studentGrade = (GradeCategory)Enum.Parse(typeof(GradeCategory), HttpContext.Session.GetString("Grade"));

            Exam exam = new()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                TotalPoints = model.TotalPoints,
                Duration = duration,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                ExamUrl = GenerateExamUrl(),
                CreatedByUserId = (Guid)await GetCurentUserId(),
                IsPublic = model.IsPublic,
                StudentGrade = studentGrade,
                StudySubject = studySubject,
                IsPublished = false,
            };

            if (exam.Description == null)
            {
                exam.Description = string.Empty;
            }

            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Exam with Id {exam.Id} was successfully created.");

            return RedirectToAction("My", "Exam");
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Preview/{examUrl}/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string ExamUrl)
        {
            var currentExam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamUrl == ExamUrl);

            if (currentExam == null)
            {
                return NotFound();
            }

            var model = new FullExamViewModel()
            {
                Title = currentExam!.Title,
                Description = currentExam.Description,
                TotalPoints = currentExam.TotalPoints,
                Duration = (int)currentExam.Duration.TotalMinutes,
                LastModifiedDate = currentExam.LastModifiedDate,
                IsPublished = currentExam.IsPublished,
                IsPublic = currentExam.IsPublic,
                ExamUrl = ExamUrl
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Preview/{examUrl}/Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(FullExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var exam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamUrl == model.ExamUrl);

            if (exam == null)
            {
                return NotFound();
            }

            int totalQuestionPoints = await _context.Questions
                .Where(q => q.ExamId == exam.Id)
                .SumAsync(q => q.Points);

            TimeSpan duration = TimeSpan.FromMinutes(model.Duration);

            exam!.Title = model.Title;
            exam.Description = model.Description;
            exam.Duration = duration;
            exam.LastModifiedDate = DateTime.Now;
            exam.IsPublic = model.IsPublic;

            if (exam.Description == null)
            {
                exam.Description = string.Empty;
            }

            var totalQuestions = await _context.Questions.CountAsync(q => q.ExamId == exam.Id);
            bool hasQuestionWithoutAnswer = await _context.Questions
                .Where(q => q.ExamId == exam.Id).AnyAsync(q => !q.Answers.Any());

            if (totalQuestions == 0 || hasQuestionWithoutAnswer)
            {
                if (model.IsPublished)
                {
                    ModelState.AddModelError(string.Empty, $"Вашият изпит в момента не разполага с формулирани въпроси или има въпрос, който не притежава зададени отговори. " +
                                     $"Моля, направете необходимите корекции. При настоящите обстоятелства, изпитът не може да бъде със статус 'Активен'");
                    return View(model);
                }
            }

            if (totalQuestionPoints > model.TotalPoints)
            {
                ModelState.AddModelError(string.Empty, $"Точките трябва да са повече или равни на {totalQuestionPoints}. " +
                    $"Това е вече зададения общ брой точки като сума на всички въпроси към този изпит");
                return View(model);
            }

            if (totalQuestionPoints < model.TotalPoints
                || exam.TotalPoints < model.TotalPoints
                || totalQuestionPoints == 0)
            {
                exam.TotalPoints = model.TotalPoints;
                exam.IsPublished = false;
            }

            else if (exam.TotalPoints == model.TotalPoints
                || totalQuestionPoints == model.TotalPoints)
            {
                exam.TotalPoints = model.TotalPoints;
                exam.IsPublished = model.IsPublished;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Exam with Id {exam.Id} was successfully edited.");

            return RedirectToAction("Preview", "Exam", new { examUrl = model.ExamUrl });
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var myExams = await _context.Exams
                .Where(t => t.CreatedByUserId == user.Id)
                .ToListAsync();

            var myExamsCollection = myExams
                .Select(e => new FullExamViewModel
                {
                    Title = e.Title,
                    Description = e.Description,
                    Duration = (int)e.Duration.TotalMinutes,
                    TotalPoints = e.TotalPoints,
                    IsPublished = e.IsPublished,
                    CreationDate = e.CreationDate,
                    ExamUrl = e.ExamUrl,
                    CreatedByUserId = e.CreatedByUserId,
                    IsPublic = e.IsPublic
                }).ToList();

            return View(myExamsCollection);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var allExams = await _context.Exams.ToListAsync();

            if (allExams == null)
            {
                return NotFound();
            }

            var allExamsCollection = allExams
                .Select(e => new FullExamViewModel
                {
                    Title = e.Title,
                    Description = e.Description,
                    Duration = (int)e.Duration.TotalMinutes,
                    TotalPoints = e.TotalPoints,
                    IsPublished = e.IsPublished,
                    CreationDate = e.CreationDate,
                    ExamUrl = e.ExamUrl,
                    CreatedByUserId = e.CreatedByUserId,
                    IsPublic = e.IsPublic

                }).ToList();

            return View(allExamsCollection);
        }

        [Route("/Exam/Preview/{examUrl}")]
        [HttpGet]
        public async Task<IActionResult> Preview(string ExamUrl)
        {
            var currentExam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamUrl == ExamUrl);

            if (currentExam == null)
            {
                return NotFound();
            }

            var loggedUser = await _userManager.GetUserAsync(User);
            if (currentExam.CreatedByUserId != loggedUser?.Id && !currentExam.IsPublished)
            {
                return NotFound();
            }

            var examPreview = new FullExamViewModel
            {
                Title = currentExam.Title,
                Description = currentExam.Description,
                TotalPoints = currentExam.TotalPoints,
                Duration = (int)currentExam.Duration.TotalMinutes,
                CreationDate = currentExam.CreationDate,
                CreatedByUserId = currentExam.CreatedByUserId,
                LastModifiedDate = currentExam.LastModifiedDate,
                IsPublished = currentExam.IsPublished,
                ExamUrl = currentExam.ExamUrl,
                IsPublic = currentExam.IsPublic,
                StudySubject = currentExam.StudySubject,
                StudentGrade = currentExam.StudentGrade,
                TimesPassed = currentExam.TimesPassed,
            };

            return View(examPreview);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Preview/{examUrl}/Open")]
        [HttpGet]
        public async Task<IActionResult> Open(string examUrl)
        {
            var currentExam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

            if (currentExam == null)
            {
                return NotFound();
            }

            var model = new FullExamViewModel()
            {
                Title = currentExam.Title,
                Description = currentExam.Description,
                TotalPoints = currentExam.TotalPoints,
                Duration = (int)currentExam.Duration.TotalMinutes,
                CreationDate = currentExam.CreationDate,
                LastModifiedDate = currentExam.LastModifiedDate,
                IsPublished = currentExam.IsPublished,
                IsPublic = currentExam.IsPublic,
                ExamUrl = examUrl,
                CreatedByUserId = currentExam.CreatedByUserId,
                TimesPassed = currentExam.TimesPassed,
                StudySubject = currentExam.StudySubject,
                StudentGrade = currentExam.StudentGrade,

                Questions = (await _context.Questions
                    .Where(q => q.ExamId == currentExam.Id)
                    .Include(q => q.Answers)
                    .ToListAsync())
                    .Select(q => new QuestionViewModel
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        QuestionType = q.QuestionType,
                        IsMultipleCorrect = q.IsMultipleCorrect,
                        Points = q.Points,
                        ExamId = q.ExamId,
                        Explanation = q.Explanation ?? string.Empty,

                        Answers = q.Answers
                            .Select(a => new AnswerViewModel
                            {
                                Id = a.Id,
                                AnswerText = a.AnswerText,
                                IsCorrect = a.IsCorrect,
                                QuestionId = a.QuestionId,
                            }).ToList()
                    }).ToList()
            };

            int totalQuestionsPoints = 0;
            bool hasQuestionWithoutAnswer = false;
            foreach (var question in model.Questions)
            {
                totalQuestionsPoints += question.Points;
                if (!question.Answers.Any())
                {
                    hasQuestionWithoutAnswer = true;
                }
            }

            ViewBag.TotalQuestionsPoints = totalQuestionsPoints;
            ViewBag.HasQuestionWithoutAnswer = hasQuestionWithoutAnswer;

            TempData["ExamQuestionTitle"] = currentExam.Title;
            TempData["ExamQuestionId"] = currentExam.Id;
            TempData["ExamQuestionUrl"] = currentExam.ExamUrl;

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Pass/{examUrl}")]
        [HttpGet]
        public async Task<IActionResult> Pass(string examUrl)
        {
            var currentExam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

            if (currentExam == null)
            {
                return NotFound();
            }

            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return NotFound();
            }

            var startTime = DateTime.Now;

            var model = new FullExamViewModel()
            {
                Title = currentExam.Title,
                Description = currentExam.Description,
                TotalPoints = currentExam.TotalPoints,
                Duration = (int)currentExam.Duration.TotalMinutes,
                CreationDate = currentExam.CreationDate,
                LastModifiedDate = currentExam.LastModifiedDate,
                IsPublished = currentExam.IsPublished,
                IsPublic = currentExam.IsPublic,
                ExamUrl = examUrl,
                CreatedByUserId = currentExam.CreatedByUserId,
                TimesPassed = currentExam.TimesPassed,
                StudySubject = currentExam.StudySubject,
                StudentGrade = currentExam.StudentGrade,

                Questions = (await _context.Questions
                    .Where(q => q.ExamId == currentExam.Id)
                    .Include(q => q.Answers)
                    .ToListAsync())
                    .Select(q => new QuestionViewModel
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        QuestionType = q.QuestionType,
                        IsMultipleCorrect = q.IsMultipleCorrect,
                        Points = q.Points,
                        ExamId = q.ExamId,
                        Explanation = q.Explanation ?? string.Empty,

                        Answers = q.Answers
                            .Select(a => new AnswerViewModel
                            {
                                Id = a.Id,
                                AnswerText = a.AnswerText,
                                IsCorrect = a.IsCorrect,
                                QuestionId = a.QuestionId,
                            }).ToList(),
                    }).ToList(),
            };

            TempData["ExamStarTime"] = DateTime.Now;
            TempData["ExamQuestionId"] = currentExam.Id;
            TempData["ExamQuestionUrl"] = currentExam.ExamUrl;
            TempData["ExamTotalPoints"] = currentExam.TotalPoints;

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Pass/{examUrl}")]
        [HttpPost]
        public async Task<IActionResult> Result(string examUrl, ExamAttemptViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentExam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

            if (currentExam == null)
            {
                return NotFound();
            }

            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return NotFound();
            }

            List<QuestionViewModel> questions = await _context.Questions
                .Include(q => q.Answers)
                .Where(q => q.ExamId == currentExam.Id)
                .Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    IsMultipleCorrect = q.IsMultipleCorrect,
                    Points = q.Points,
                    ExamId = q.ExamId,
                    Explanation = q.Explanation ?? string.Empty,

                    Answers = q.Answers.Select(a => new AnswerViewModel
                    {
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                })
                .ToListAsync();

            var startTime = TempData["ExamStarTime"];
            var durationTaken = DateTime.Now - (DateTime)startTime!;

            var newExamAttempt = new ExamAttempt
            {
                Id = Guid.NewGuid(),
                StartTime = (DateTime)startTime!,
                EndTime = DateTime.Now,
                DurationTaken = durationTaken,
                IsCompleted = true,
                ExamId = currentExam.Id,
                UserId = loggedUser.Id
            };

            newExamAttempt.Score = CalculateTotalScoreInPercentage(questions);

            _context.ExamAttempts.Add(newExamAttempt);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // Methods used in class: ExamController
        private async Task<Guid?> GetCurentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && Guid.TryParse(userIdClaim, out Guid parsedUserId))
            {
                return await Task.FromResult(parsedUserId);
            }
            else
            {
                return await Task.FromResult<Guid?>(null);
            }
        }

        private double CalculateTotalScoreInPercentage(List<QuestionViewModel> questions)
        {
            int sumQuestionsScore = 0;
            int maxPossiblePoints = (int)TempData["ExamTotalPoints"]!;

            foreach (var question in questions)
            {
                if (question.Answers.Any(a => a.IsCorrect))
                {
                    if (question.QuestionType == "TrueFalse")
                    {
                        sumQuestionsScore += question.Points;
                    }
                    if (question.QuestionType == "Multiple")
                    {
                        if (question.IsMultipleCorrect)
                        {
                            int totalQuestions = question.Answers.Count();
                            int totalTrueQuestions = question.Answers.Count(q => q.IsCorrect);

                            foreach (var answer in question.Answers)
                            {
                                if (answer.IsCorrect)
                                {
                                    sumQuestionsScore += question.Points / totalTrueQuestions;
                                } 
                            }
                        }
                        else
                        {
                            sumQuestionsScore += question.Points;
                        }   
                    }

                }
            }

            if (sumQuestionsScore == 0)
            {
                return 0;
            }

            double scoreInPercentage = sumQuestionsScore / maxPossiblePoints * 100;

            return Math.Round(scoreInPercentage, MidpointRounding.AwayFromZero);
        }
    }
}
