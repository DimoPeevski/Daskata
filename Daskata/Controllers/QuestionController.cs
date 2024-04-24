using Daskata.Core.Contracts.Question;
using Daskata.Core.ViewModels;
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
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionService _questionService;

        public QuestionController (UserManager<UserProfile> userManager,
            ILogger<QuestionController> logger,
            IQuestionService questionService)
        {
            _userManager = userManager;
            _logger = logger;
            _questionService = questionService;
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Question/{parentExamUrl}/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var parentExamUrl = TempData["ExamQuestionUrl"];

            var model = await _questionService.PrepareQuestionForCreationAsync(parentExamUrl!.ToString()!);

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

            try

            {
                await _questionService.CreateQuestionAsync(model, parentExamUrl);

                _logger.LogInformation("New question was created for the exam URL: {parentExamUrl}", parentExamUrl);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error creating question: Exam not found for URL: {parentExamUrl}", parentExamUrl);
                ModelState.AddModelError(string.Empty, "Търсеният изпит не беше намерен.");
                return View(model);
            }

            return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl });
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Question/{parentExamUrl}/{questionId}/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid questionId)
        {
            var parentExamId = TempData["ExamQuestionId"] as Guid?;

            if (parentExamId == null)
            {
                return NotFound();
            }

            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return Unauthorized();
            }

            try
            {
                var model = await _questionService.GetQuestionForEditAsync(questionId, loggedUser.Id);
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpPost]
        [Route("/Question/{parentExamUrl}/{questionId}/Edit")]
        public async Task<IActionResult> EditQuestion(QuestionViewModel model, string parentExamUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _questionService.UpdateQuestionAsync(model, parentExamUrl);

                if (!result)
                {
                    return NotFound();
                }

                _logger.LogInformation($"Question with Id {model.Id} was successfully edited.");
                return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when trying to update the question.");
                ModelState.AddModelError(string.Empty, "Възникна неочаквана грешка по време на актуализиране на въпроса.");
                return View(model);
            }
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Question/{parentExamUrl}/{questionId}/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string parentExamUrl, Guid questionId)
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return Unauthorized();
            }

            var result = await _questionService.DeleteQuestionAsync(questionId, loggedUser.Id);

            if (!result)
            {
                return NotFound();
            }

            _logger.LogInformation($"Question with Id {questionId} was successfully deleted.");

            return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl });
        }
    }
}

