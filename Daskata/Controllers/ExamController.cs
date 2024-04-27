using Daskata.Core.Contracts.Exam;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Daskata.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<ExamController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExamService _examService;

        public ExamController(UserManager<UserProfile> userManager,
            ILogger<ExamController> logger,
            IHttpContextAccessor httpContextAccessor,
            IExamService examService)
        {
            _userManager = userManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _examService = examService;
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("All", "Exam");
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var subjectList = await _examService.Create();
            return View(subjectList);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Create/Grade")]
        [HttpPost]
        public async Task<IActionResult> Grade([FromForm] string subject)
        {
            var gradeList = await _examService.Grade(subject);

            HttpContext.Session.SetString("Subject", subject);
            return View(gradeList);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Create/Grade/Details")]
        [HttpPost]
        public async Task<IActionResult> Details([FromForm] string grade)
        {
            var model = await _examService.Details(grade);

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

            var subject = HttpContext.Session.GetString("Subject");
            var grade = HttpContext.Session.GetString("Grade");
            var userId = (Guid)await GetCurentUserId();

            var exam = await _examService.Publish(model, subject, grade, userId);

            _logger.LogInformation($"Exam with Id {exam.Id} was successfully created.");

            return RedirectToAction("My", "Exam");
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Preview/{examUrl}/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string ExamUrl)
        {
            Exam currentExam;
            try
            {
                currentExam = await _examService.GetExamByUrlAsync(ExamUrl);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting exam.");
                return BadRequest();
            }

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

            try
            {
                var exam = await _examService.UpdateExamAsync(model);

                _logger.LogInformation($"Exam with Id {exam.Id} was successfully edited.");

                return RedirectToAction("Preview", "Exam", new { examUrl = model.ExamUrl });
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            try
            {
                var myExamsCollection = await _examService.GetExamsByCreatorAsync(user.Id);
                return View(myExamsCollection);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user's exams.");
                return NotFound();
            }
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public async Task<IActionResult> All(int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                // Make sure the GetAllExamsAsync method returns a PaginatedList<FullExamViewModel>
                var paginatedExams = await _examService.GetAllExamsAsync(pageNumber, pageSize);

                if (paginatedExams == null || !paginatedExams.Any())
                {
                    return NotFound();
                }

                // The ViewModel should reflect pagination, so pass the PaginatedList<FullExamViewModel> to the view
                var viewModel = new ExamListViewModel
                {
                    Exams = paginatedExams,
                    CurrentPage = pageNumber,
                    TotalPages = paginatedExams.TotalPages
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all exams.");
                return NotFound();
            }
        }

        [Route("/Exam/Preview/{examUrl}")]
        [HttpGet]
        public async Task<IActionResult> Preview(string examUrl)
        {
            var examPreview = await _examService.GetExamPreview(examUrl);

            if (examPreview == null)
            {
                return NotFound();
            }

            return View(examPreview);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Preview/{examUrl}/Open")]
        [HttpGet]
        public async Task<IActionResult> Open(string examUrl)
        {
            var model = await _examService.GetOpenExam(examUrl);

            if (model == null)
            {
                return NotFound();
            }

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

            TempData["ExamQuestionTitle"] = model.Title;
            TempData["ExamQuestionId"] = model.Id;
            TempData["ExamQuestionUrl"] = model.ExamUrl;

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Pass/{examUrl}")]
        [HttpGet]
        public async Task<IActionResult> Pass(string examUrl)
        {
            FullExamViewModel model;
            try
            {
                model = await _examService.GetExamAndQuestionsByUrlAsync(examUrl);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting exam.");
                return BadRequest();
            }

            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return NotFound();
            }

            TempData["ExamStarTime"] = DateTime.Now;
            TempData["ExamQuestionId"] = model.Id;
            TempData["ExamQuestionUrl"] = model.ExamUrl;
            TempData["ExamTotalPoints"] = model.TotalPoints;

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

            try
            {
                var loggedUser = await _userManager.GetUserAsync(User);

                if (loggedUser == null)
                {
                    return NotFound();
                }

                model.UserId = loggedUser.Id;
                var newExamAttempt = await _examService.CalculateExamResultAsync(examUrl, model);

                return RedirectToAction("Index", "Home");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating exam result.");
                return BadRequest();
            }
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
    }
}
