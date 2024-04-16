using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly UserManager<UserProfile> _userManager;
        private readonly DaskataDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionController(ILogger<QuestionController> logger,
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
        [Route("/Question/{parentExamUrl}/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var parentExamUrl = TempData["ExamQuestionUrl"];

            var parentExamId = TempData["ExamQuestionId"] as Guid?;
            if (parentExamId == null)
            {
                return NotFound();
            }

            var parentExam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == parentExamId.Value);

            var model = new QuestionViewModel
            {
                Id = Guid.NewGuid(),
                QuestionText = string.Empty,
                QuestionType = "",
                IsMultipleCorrect = false,
                Points = 10,
                ExamId = parentExamId.Value,
                Explanation = string.Empty,
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpPost]
        [Route("/Question/{parentExamUrl}/Create")]
        public async Task<IActionResult> Create(QuestionViewModel model, string parentExamUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentExam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamUrl == parentExamUrl);

            if (currentExam == null)
            {
                return NotFound();
            }

            var question = new Question
            {
                Id = Guid.NewGuid(),
                QuestionText = model.QuestionText,
                QuestionType = model.QuestionType,
                IsMultipleCorrect = model.IsMultipleCorrect,
                Points = model.Points,
                ExamId = currentExam.Id,
                Explanation = model.Explanation,
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl });
        }
    }
}

