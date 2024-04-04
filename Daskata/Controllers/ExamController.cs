using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Daskata.Controllers
{
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            var model = new CreateExamFormModel()
            {
                Title = string.Empty,
                Description = string.Empty,
                TotalPoints = 120,
                Duration = TimeSpan.FromMinutes(60),
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                IsPublished = false,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateExamFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Exam exam = new()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                TotalPoints = model.TotalPoints,
                Duration = TimeSpan.FromMinutes(60),
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                IsPublished = false,
                CreatedByUserId = (Guid)await GetCurentUserId()
            };

            if (exam.Description == null)
            {
                exam.Description = string.Empty;
            }



            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();

            _logger.LogInformation("The exam was created successfully");
            return RedirectToAction("My", "Exam");
        }

        [Authorize]
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
                .Select(e => new ExamCollectionViewModel
                {
                    Title = e.Title,
                    Description = e.Description,
                    Duration = e.Duration,
                    TotalPoints = e.TotalPoints,
                    IsPublished = e.IsPublished,
                    CreationDate = e.CreationDate

                }).ToList();

            return View(myExamsCollection);
        }

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
