using Daskata.Core.Contracts.Answer;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Core.Services.Answer
{
    public class AnswerService : IAnswerService
    {
        private readonly IRepository _repository;

        public AnswerService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<AnswerViewModel> PrepareAnswerForCreationAsync(Guid questionId, Guid userId)
        {
            var question = await _repository.All<Infrastructure.Data.Models.Question>()
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
            {
                throw new InvalidOperationException("Въпросът не е намерен.");
            }

            var exam = await _repository.All<Exam>()
                .FirstOrDefaultAsync(e => e.Id == question.ExamId);

            if (exam == null || exam.CreatedByUserId != userId)
            {
                throw new InvalidOperationException("Изпитът не е намерен.");
            }

            var model = new AnswerViewModel
            {
                Id = Guid.NewGuid(),
                AnswerText = string.Empty,
                IsCorrect = false,
                QuestionId = questionId,
                ParentExamUrl = exam.ExamUrl
            };

            return model;
        }

        public async Task AddAnswerAsync(AnswerViewModel model, Guid questionId, Guid userId)
        {
            var question = await _repository.All<Infrastructure.Data.Models.Question>()
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
            {
                throw new InvalidOperationException("Въпросът не беше намерен.");
            }

            ValidateAnswerForQuestion(model, question, question.Answers);

            var answer = new Infrastructure.Data.Models.Answer
            {
                Id = Guid.NewGuid(),
                AnswerText = model.AnswerText,
                IsCorrect = model.IsCorrect,
                QuestionId = question.Id,
            };

            await _repository.AddAsync(answer);
            await _repository.SaveChangesAsync();
        }

        public async Task<AnswerViewModel> GetAnswerForEditAsync(Guid answerId, Guid userId)
        {
            var answer = await _repository.All<Infrastructure.Data.Models.Answer>()
                .Include(a => a.Question)
                .ThenInclude(q => q.Exam)
                .FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                throw new InvalidOperationException("Отговорът не е намерен.");
            }

            if (answer.Question.Exam.CreatedByUserId != userId)
            {
                throw new UnauthorizedAccessException("Потребителят няма права да редактира този отговор.");
            }

            var model = new AnswerViewModel
            {
                Id = answer.Id,
                AnswerText = answer.AnswerText,
                IsCorrect = answer.IsCorrect,
                QuestionId = answer.QuestionId,
                ParentExamUrl = answer.Question.Exam.ExamUrl,
            };

            return model;
        }

        public async Task EditAnswerAsync(AnswerViewModel model, Guid answerId, Guid userId)
        {
            var answer = await _repository.All<Infrastructure.Data.Models.Answer>()
                .Include(a => a.Question)
                .ThenInclude(q => q.Exam)
                .FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                throw new InvalidOperationException("Отговорът не беше намерен.");
            }

            if (answer.Question.Exam.CreatedByUserId != userId)
            {
                throw new UnauthorizedAccessException("Потребителят няма права да редактира отговора.");
            }

            var otherAnswers = answer.Question.Answers.Where(a => a.Id != answerId).ToList();

            ValidateAnswerForQuestion(model, answer.Question, otherAnswers);

            answer.AnswerText = model.AnswerText;
            answer.IsCorrect = model.IsCorrect;

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(Guid answerId, Guid userId)
        {
            var answer = await _repository.All<Infrastructure.Data.Models.Answer>()
                .Include(a => a.Question)
                .FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                throw new InvalidOperationException("Въпросът не беше намерен.");
            }

            var parentExam = await _repository.All<Exam>()
                .FirstOrDefaultAsync(e => e.Id == answer.Question.ExamId);

            if (parentExam!.CreatedByUserId != userId)
            {
                throw new UnauthorizedAccessException("Потребителят няма права да редактира отговора.");
            }

            _repository.Remove(answer);

            await _repository.SaveChangesAsync();
        }

        public void ValidateAnswerForQuestion(AnswerViewModel model, Infrastructure.Data.Models.Question question, IEnumerable<Infrastructure.Data.Models.Answer> otherAnswers)
        {
            bool isCurrentlyTrue = model.IsCorrect;

            int trueAnswerCount = otherAnswers.Count(a => a.IsCorrect) + (isCurrentlyTrue ? 1 : 0);

            if (question.QuestionType == "TrueFalse")
            {
                if (trueAnswerCount > 1)
                {
                    throw new ArgumentException("Въпроси от тип 'Правилно/Грешно' трябва да имат само един правилен отговор.");
                }
                if (otherAnswers.Count() > 1)
                {
                    throw new ArgumentException("Въпроси от тип 'Правилно/Грешно' трябва да имат не повече от два отговора общо.");

                }
            }

            if (question.QuestionType == "Multiple" && !question.IsMultipleCorrect)
            {
                if (trueAnswerCount > 0)
                {
                    throw new ArgumentException("Въпросът трябва да има само един правилен отговор.");
                }
            }
        }
    }
}
