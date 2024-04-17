using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Controllers
{
    public class AnswerController : Controller
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly UserManager<UserProfile> _userManager;
        private readonly DaskataDbContext _context;

        public AnswerController(ILogger<AnswerController> logger,
                              IHttpContextAccessor httpContextAccessor,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/Add")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var parentExamUrl = TempData["ExamQuestionUrl"];
            var parentExamId = TempData["ExamQuestionId"] as Guid?;

            if (parentExamUrl == null || parentExamId == null)
            {
                return NotFound();
            }

            var loggedUser = await _userManager.GetUserAsync(User);
            var parentExam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == parentExamId.Value);

            if (loggedUser == null || parentExam!.CreatedByUserId != loggedUser!.Id)
            {
                return NotFound();
            }

            var model = new AnswerViewModel
            {
                Id = Guid.NewGuid(),
                AnswerText = string.Empty,
                IsCorrect = false,
                QuestionId = parentExamId.Value,
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/Add")]
        [HttpPost]
        public async Task<IActionResult> AddAnswer(
            string parentExamUrl, 
            Guid parentQuestionId, 
            AnswerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var question = await _context.Questions
                .Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == parentQuestionId);

            if (question == null)
            {
                return NotFound();
            }

            var answer = new Answer
            {
                Id = Guid.NewGuid(),
                AnswerText = model.AnswerText,
                IsCorrect = model.IsCorrect,
                QuestionId = question.Id
            };

            if (question.QuestionType == "TrueFalse")
            {
                if (question.Answers.Count() >= 2)
                {
                    ModelState.AddModelError(string.Empty, "Въпроси от тип 'Правилно/Грешно' могат да имат само по два отговора.");
                    return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
                }

                if (question.Answers.Count(a => a.IsCorrect) > 1)
                {
                    ModelState.AddModelError(string.Empty, "Въпросът трябва да има точно един правилен отговор. Моля направете корекция");
                    return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
                }
            }

            if (question.QuestionType == "Multiple" && !question.IsMultipleCorrect)
            {
                if (question.Answers.Count(a => a.IsCorrect) > 1)
                {
                    ModelState.AddModelError(string.Empty, "Въпросът трябва да има точно един правилен отговор. Моля направете корекция");
                    return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
                }
            }


            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Answer with Id {answer.Id} was successfully created");

            return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/{answerId}/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string parentExamUrl, Guid parentQuestionId, Guid answerId)
        {
            var parentExamId = TempData["ExamQuestionId"] as Guid?;

            if (parentExamId == null)
            {
                return NotFound();
            }

            var parentExam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == parentExamId.Value);
            var loggedUser = await _userManager.GetUserAsync(User);

            if (parentExam == null || loggedUser == null)
            {
                return NotFound();
            }

            if (parentExam.CreatedByUserId != loggedUser.Id)
            {
                return Unauthorized();
            }

            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                return NotFound();
            }

            var model = new AnswerViewModel
            {
                Id = answer.Id,
                AnswerText = answer.AnswerText,
                IsCorrect = answer.IsCorrect,
                QuestionId = answer.QuestionId
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/{answerId}/Edit/")]
        [HttpPost]
        public async Task<IActionResult> Edit(string parentExamUrl, Guid parentQuestionId, 
            Guid answerId, AnswerViewModel model)
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

            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                return NotFound();
            }

            answer.AnswerText = model.AnswerText;
            answer.IsCorrect = model.IsCorrect;

            var question = await _context.Questions
                .Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == parentQuestionId);

            if (question == null)
            {
                return NotFound();
            }

            if (question.QuestionType == "TrueFalse" && question.Answers.Count(a => a.IsCorrect) > 1)
            {
                ModelState.AddModelError(string.Empty, "Въпросът вече има зададен правилен отговор. Моля направете корекция");
                return View(model);
            }

            if (question.QuestionType == "Multiple" && !question.IsMultipleCorrect)
            {
                if (question.Answers.Count(a => a.IsCorrect) > 1)
                {
                    ModelState.AddModelError(string.Empty, "Въпросът вече има зададен правилен отговор. Моля направете корекция");
                    return View(model);
                }
            }

            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Answer with Id {answer.Id} was successfully edited");

            return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [Route("/Answer/{parentExamUrl}/{parentQuestionId}/{answerId}/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string parentExamUrl, Guid parentQuestionId, Guid answerId)
        {
            var parentExamId = TempData["ExamQuestionId"] as Guid?;

            if (parentExamId == null)
            {
                return NotFound();
            }

            var parentExam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == parentExamId.Value);
            var loggedUser = await _userManager.GetUserAsync(User);

            if (parentExam == null || loggedUser == null)
            {
                return NotFound();
            }

            if (parentExam.CreatedByUserId != loggedUser.Id)
            {
                return Unauthorized();
            }

            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(answer);

            int answersCount = await _context.Answers
                .Where(a => a.QuestionId == parentQuestionId).CountAsync();

            if (answersCount == 0)
            {
                parentExam.IsPublished = false;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Answer with Id {answer.Id} was successfully deleted.");

            return RedirectToAction("Open", "Exam", new { examUrl = parentExamUrl, id = parentQuestionId });
        }
    }
}
