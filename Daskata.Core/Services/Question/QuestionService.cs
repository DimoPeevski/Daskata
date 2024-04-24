using Daskata.Core.Contracts.Question;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Daskata.Core.Services.Question
{
    public class QuestionService : IQuestionService
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<QuestionService> _logger;
        private readonly IRepository _repository;

        public QuestionService(UserManager<UserProfile> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<QuestionService> logger,
            IRepository repository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _repository = repository;
        }

        public async Task<QuestionViewModel> PrepareQuestionForCreationAsync(string parentExamUrl)
        {
            var userClaimsPrincipal = _httpContextAccessor.HttpContext?.User;
            var loggedUser = await _userManager.GetUserAsync(userClaimsPrincipal);

            if (loggedUser == null)
            {
                _logger.LogError("No logged-in user found.");
                return null;
            }

            var parentExam = await _repository.All<Exam>()
                .FirstOrDefaultAsync(e => e.ExamUrl == parentExamUrl);

            if (parentExam == null || parentExam.CreatedByUserId != loggedUser.Id)
            {
                _logger.LogError($"Exam with Id {parentExam.Id} not found, or user not authorized.");

                return null;
            }

            var model = new QuestionViewModel
            {
                Id = Guid.NewGuid(),
                QuestionText = string.Empty,
                QuestionType = "",
                IsMultipleCorrect = false,
                Points = 10,
                ExamId = parentExam.Id,
                Explanation = string.Empty,
            };

            return model;
        }

        public async Task<Guid> CreateQuestionAsync(QuestionViewModel model, string examUrl)
        {
            var currentExam = await _repository.All<Exam>()
           .FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

            if (currentExam == null)
            {
                throw new ArgumentException("Изпитът не е намерен.");
            }

            var question = new Infrastructure.Data.Models.Question
            {
                Id = Guid.NewGuid(),
                QuestionText = model.QuestionText,
                QuestionType = model.QuestionType,
                IsMultipleCorrect = model.IsMultipleCorrect,
                Points = model.Points,
                ExamId = currentExam.Id,
                Explanation = model.Explanation,
            };

            await _repository.AddAsync(question);
            await _repository.SaveChangesAsync();

            return question.Id;
        }

        public async Task<QuestionViewModel> GetQuestionForEditAsync(Guid questionId, Guid userId)
        {
            var question = await _repository.All<Infrastructure.Data.Models.Question>()
                .Include(q => q.Exam)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
            {
                throw new InvalidOperationException("Въпросът не може да бъде намерен.");
            }

            if (question.Exam.CreatedByUserId != userId)
            {
                throw new UnauthorizedAccessException("Нямате права да редактирате този въпрос");
            }

            var model = new QuestionViewModel
            {
                Id = question.Id,
                QuestionText = question.QuestionText,
                QuestionType = question.QuestionType,
                IsMultipleCorrect = question.IsMultipleCorrect,
                Points = question.Points,
                ExamId = question.ExamId,
                Explanation = question.Explanation
            };

            return model;
        }

        public async Task<bool> UpdateQuestionAsync(QuestionViewModel model, string examUrl)
        {
            var currentExam = await _repository.All<Exam>()
                .FirstOrDefaultAsync(e => e.ExamUrl == examUrl);

            if (currentExam == null)
            {
                throw new KeyNotFoundException("Изпитът не беше намерен.");
            }

            var question = await _repository.All<Infrastructure.Data.Models.Question>()
                .FirstOrDefaultAsync(q => q.Id == model.Id);

            if (question == null)
            {
                throw new KeyNotFoundException("Въпросът не беше намерен.");
            }

            question.QuestionText = model.QuestionText;
            question.QuestionType = model.QuestionType;
            question.IsMultipleCorrect = model.IsMultipleCorrect;
            question.Points = model.Points;
            question.Explanation = model.Explanation;

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteQuestionAsync(Guid questionId, Guid userId)
        {
            var question = await _repository.All<Infrastructure.Data.Models.Question>()
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
            {
                return false;
            }

            var exam = await _repository.All<Exam>().FirstOrDefaultAsync(e => e.Id == question.ExamId);

            if (exam == null || exam.CreatedByUserId != userId)
            {
                return false;
            }

            _repository.Remove(question);

            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
