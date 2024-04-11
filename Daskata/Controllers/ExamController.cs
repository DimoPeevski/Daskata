using Daskata.Core.ViewModels;
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
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<UserProfile> _userManager;
        private readonly DaskataDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExamController(ILogger<HomeController> logger,
                              IHttpContextAccessor httpContextAccessor,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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

            };

            return View(examPreview);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new FullExamViewModel()
            {
                Title = string.Empty,
                Description = string.Empty,
                Duration = 30,
                TotalPoints = 60,
                IsPublished = false,
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(FullExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TimeSpan duration = TimeSpan.FromMinutes(model.Duration);

            Exam exam = new()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                TotalPoints = model.TotalPoints,
                Duration = duration,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                IsPublished = model.IsPublished,
                ExamUrl = GenerateExamUrl(),
                CreatedByUserId = (Guid)await GetCurentUserId()
            };

            if (exam.Description == null)
            {
                exam.Description = string.Empty;
            }

            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();

            _logger.LogInformation("The exam was created successfully.");
            return RedirectToAction("My", "Exam");
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Exam/Preview/{examUrl}/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string ExamUrl)
        {
            var currentExam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamUrl == ExamUrl);

            if(currentExam == null)
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

            TimeSpan duration = TimeSpan.FromMinutes(model.Duration);

            exam!.Title = model.Title;
            exam.Description = model.Description;
            exam.TotalPoints = model.TotalPoints;
            exam.Duration = duration;
            exam.LastModifiedDate = DateTime.Now;
            exam.IsPublished = model.IsPublished;

            if (exam.Description == null)
            {
                exam.Description = string.Empty;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("The exam was edited successfully.");
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
                .Select(e => new PartialExamViewModel
                {
                    Title = e.Title,
                    Description = e.Description,
                    Duration = e.Duration,
                    TotalPoints = e.TotalPoints,
                    IsPublished = e.IsPublished,
                    CreationDate = e.CreationDate,
                    ExamUrl = e.ExamUrl,
                    CreatedByUserId = e.CreatedByUserId

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
                .Select(e => new PartialExamViewModel
                {
                    Title = e.Title,
                    Description = e.Description,
                    Duration = e.Duration,
                    TotalPoints = e.TotalPoints,
                    IsPublished = e.IsPublished,
                    CreationDate = e.CreationDate,
                    ExamUrl = e.ExamUrl,
                    CreatedByUserId = e.CreatedByUserId

                }).ToList();

            return View(allExamsCollection);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("All","Exam");
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
