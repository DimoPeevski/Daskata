using Daskata.Core.Contracts.Answer;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Daskata.Controllers
{
    public class AnswerController : Controller
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<AnswerController> _logger;
        private readonly IAnswerService _answerService;

        public AnswerController(UserManager<UserProfile> userManager,
            ILogger<AnswerController> logger,
            IAnswerService answerService)
        {
            _userManager = userManager;
            _logger = logger;
            _answerService = answerService;
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/Add")]
        [HttpGet]
        public async Task<IActionResult> Add(Guid parentQuestionId)
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return Unauthorized();
            }

            try
            {
                var model = await _answerService.PrepareAnswerForCreationAsync(parentQuestionId, loggedUser.Id);

                if (model == null)
                {
                    return NotFound();
                }

                return View(model);
            }

            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Answer not found.");

                return NotFound();
            }

            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized attempt to open an answer.");

                return Unauthorized();
            }
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string parentExamUrl, Guid parentQuestionId, AnswerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return Unauthorized();
            }

            try
            {
                await _answerService.AddAnswerAsync(model, parentQuestionId, loggedUser.Id);

                _logger.LogInformation($"Answer with Id {model.Id} was successfully created");

                return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
            }

            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }

            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/{answerId}/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid answerId)
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return Unauthorized();
            }

            try
            {
                var model = await _answerService.GetAnswerForEditAsync(answerId, loggedUser.Id);
                if (model == null)
                {
                    return NotFound();
                }
                return View(model);
            }

            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Answer not found.");

                return NotFound();
            }

            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized attempt to edit an answer.");

                return Unauthorized();
            }
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/{answerId}/Edit/")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string parentExamUrl, Guid parentQuestionId, 
            Guid answerId, AnswerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null)
            {
                return Unauthorized();
            }

            try
            {
                await _answerService.EditAnswerAsync(model, answerId, loggedUser.Id);

                _logger.LogInformation($"Answer with Id {answerId} was successfully edited");

                return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
            }

            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }

            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/{answerId}/Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (string parentExamUrl, Guid parentQuestionId, Guid answerId)
        {
            try
            {
                var userId = (await _userManager.GetUserAsync(User))?.Id;

                if (userId == null)
                {
                    return Unauthorized();
                }

                await _answerService.DeleteAnswerAsync(answerId, userId.Value);

                _logger.LogInformation($"Answer with Id {answerId} was successfully deleted.");
                return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
            }

            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Отговорът не е намерен.");
                return NotFound();
            }

            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Потребителят няма права да изтрие отговора.");
                return Unauthorized();
            }
        }
    }
}
