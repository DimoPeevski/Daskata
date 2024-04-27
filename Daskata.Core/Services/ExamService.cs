using Daskata.Core.Contracts.Exam;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Daskata.Core.Shared.Methods;

namespace Daskata.Core.Services
{
    public class ExamService : IExamService
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository _repository;

        public ExamService(UserManager<UserProfile> userManager,
            IHttpContextAccessor httpContextAccessor,
            IRepository repository)
        {
            _repository = repository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<FullExamViewModel>> GetAllExamsAsync()
        {
            List<Infrastructure.Data.Models.Exam> exams = await _repository.All<Infrastructure.Data.Models.Exam>().ToListAsync();

            return exams.Select(e => new FullExamViewModel
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
        }

        public async Task<List<FullExamViewModel>> GetExamsByCreatorAsync(Guid userId)
        {
            var myExams = await _repository.All<Infrastructure.Data.Models.Exam>()
                .Where(t => t.CreatedByUserId == userId)
                .ToListAsync();

            return myExams.Select(e => new FullExamViewModel
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
        }

        public async Task<FullExamViewModel> GetExamPreview(string examUrl)
        {
            var currentExam = await _repository.All<Infrastructure.Data.Models.Exam>()
                .FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

            if (currentExam == null)
            {
                return null;
            }

            var loggedUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (currentExam.CreatedByUserId != loggedUser?.Id && !currentExam.IsPublished)
            {
                return null;
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

            return examPreview;
        }

        public async Task<FullExamViewModel> GetOpenExam(string examUrl)
        {
            try
            {
                var currentExam = await _repository.All<Infrastructure.Data.Models.Exam>()
                    .FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

                if (currentExam == null)
                {
                    return null;
                }

                var model = new FullExamViewModel
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

                    Questions = (await _repository.All<Infrastructure.Data.Models.Question>()
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

                return model;
            }

            catch (Exception ex)
            {
                throw new Exception ("Изпитът не е намерен.");
            }
        }

        public async Task<List<string>> Create()
        {
            var subjects = Enum.GetValues(typeof(SubjectCategory));
            var subjectList = new List<string>();
            foreach (var subject in subjects)
            {
                subjectList.Add(subject.ToString());
            }
            return subjectList;
        }
        public async Task<List<string>> Grade(string subject)
        {
            var grades = Enum.GetValues(typeof(GradeCategory));
            var gradeList = new List<string>();

            foreach (var grade in grades)
            {
                gradeList.Add(grade.ToString());
            }

            return gradeList;
        }

        public async Task<FullExamViewModel> Details(string grade)
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

            return model;
        }

        public async Task<Infrastructure.Data.Models.Exam> Publish(FullExamViewModel model, string subject, string grade, Guid userId)
        {
            TimeSpan duration = TimeSpan.FromMinutes(model.Duration);

            var studySubject = (SubjectCategory)Enum.Parse(typeof(SubjectCategory), subject);
            var studentGrade = (GradeCategory)Enum.Parse(typeof(GradeCategory), grade);

            Infrastructure.Data.Models.Exam exam = new()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description ?? string.Empty,
                TotalPoints = model.TotalPoints,
                Duration = duration,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                ExamUrl = GenerateExamUrl(),
                CreatedByUserId = userId,
                IsPublic = model.IsPublic,
                StudentGrade = studentGrade,
                StudySubject = studySubject,
                IsPublished = false,
            };

            await _repository.AddAsync(exam);
            await _repository.SaveChangesAsync();

            return exam;
        }

        public async Task<Infrastructure.Data.Models.Exam> GetExamByUrlAsync(string examUrl)
        {
            return await _repository.All<Infrastructure.Data.Models.Exam>().FirstOrDefaultAsync(e => e.ExamUrl == examUrl);
        }

        public async Task<Infrastructure.Data.Models.Exam> UpdateExamAsync(FullExamViewModel model)
        {
            var exam = await _repository.All<Infrastructure.Data.Models.Exam>().FirstOrDefaultAsync(e => e.ExamUrl == model.ExamUrl);

            if (exam == null)
            {
                throw new Exception("Exam not found");
            }

            int totalQuestionPoints = await _repository.All<Infrastructure.Data.Models.Question>()
                .Where(q => q.ExamId == exam.Id)
                .SumAsync(q => q.Points);

            TimeSpan duration = TimeSpan.FromMinutes(model.Duration);

            exam.Title = model.Title;
            exam.Description = model.Description;
            exam.Duration = duration;
            exam.LastModifiedDate = DateTime.Now;
            exam.IsPublic = model.IsPublic;

            if (exam.Description == null)
            {
                exam.Description = string.Empty;
            }

            var totalQuestions = await _repository.All<Infrastructure.Data.Models.Question>().CountAsync(q => q.ExamId == exam.Id);
            bool hasQuestionWithoutAnswer = await _repository.All<Infrastructure.Data.Models.Question>()
                .Where(q => q.ExamId == exam.Id).AnyAsync(q => !q.Answers.Any());

            if (totalQuestions == 0 || hasQuestionWithoutAnswer)
            {
                if (model.IsPublished)
                {
                    throw new Exception($"Вашият изпит в момента не разполага с формулирани въпроси или има въпрос, който не притежава зададени отговори." +
                    $"Моля, направете необходимите корекции. При настоящите обстоятелства, изпитът не може да бъде със статус 'Активен'");
                }
            }

            if (totalQuestionPoints > model.TotalPoints)
            {
                throw new Exception($"Точките трябва да са повече или равни на {totalQuestionPoints}. " +
                    $"Това е вече зададения общ брой точки като сума на всички въпроси към този изпит");
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

            await _repository.SaveChangesAsync();

            return exam;
        }

        public async Task<FullExamViewModel> GetExamAndQuestionsByUrlAsync(string examUrl)
        {
            var currentExam = await _repository.All<Infrastructure.Data.Models.Exam>()
                .FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

            if (currentExam == null)
            {
                throw new Exception("Изпитът не беше намерен.");
            }

            var questions = await _repository.All<Infrastructure.Data.Models.Question>()
                .Where(q => q.ExamId == currentExam.Id)
                .Include(q => q.Answers)
                .ToListAsync();

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

                Questions = questions.Select(q => new QuestionViewModel
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

            return model;
        }

        public async Task<ExamAttempt> CalculateExamResultAsync(string examUrl, ExamAttemptViewModel model)
        {
            var currentExam = await _repository.All<Infrastructure.Data.Models.Exam>().FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

            if (currentExam == null)
            {
                throw new Exception("Exam not found");
            }

            List<QuestionViewModel> questions = await _repository.All<Infrastructure.Data.Models.Question>()
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

            var startTime = DateTime.Now;
            var durationTaken = DateTime.Now - startTime;

            var newExamAttempt = new ExamAttempt
            {
                Id = Guid.NewGuid(),
                StartTime = startTime,
                EndTime = DateTime.Now,
                DurationTaken = durationTaken,
                IsCompleted = true,
                ExamId = currentExam.Id,
                UserId = model.UserId,
                Score = CalculateTotalScoreInPercentage(questions, currentExam.TotalPoints)
            };

            await _repository.AddAsync(newExamAttempt);
            await _repository.SaveChangesAsync();

            return newExamAttempt;
        }

        public double CalculateTotalScoreInPercentage(List<QuestionViewModel> questions, int maxPossiblePoints)
        {
            int sumQuestionsScore = 0;

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

            double scoreInPercentage = (double)sumQuestionsScore / maxPossiblePoints * 100;

            return Math.Round(scoreInPercentage, MidpointRounding.AwayFromZero);
        }
        public async Task<PaginatedList<FullExamViewModel>> GetAllExamsAsync(int pageNumber, int pageSize)
        {
            var count = await _repository.All<Infrastructure.Data.Models.Exam>().CountAsync();

            List<Infrastructure.Data.Models.Exam> exams = await _repository
                .All<Infrastructure.Data.Models.Exam>()
                .OrderBy(e => e.Title)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            List<FullExamViewModel> examViewModels = exams
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

            return new PaginatedList<FullExamViewModel>(examViewModels, count, pageNumber, pageSize);
        }
    }
}
