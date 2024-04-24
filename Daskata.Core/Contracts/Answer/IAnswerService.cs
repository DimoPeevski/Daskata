using Daskata.Core.ViewModels;

namespace Daskata.Core.Contracts.Answer
{
    public interface IAnswerService
    {
        Task<AnswerViewModel> PrepareAnswerForCreationAsync(Guid questionId, Guid userId);

        Task AddAnswerAsync(AnswerViewModel model, Guid questionId, Guid userId);

        Task<AnswerViewModel> GetAnswerForEditAsync(Guid answerId, Guid userId);

        Task EditAnswerAsync(AnswerViewModel model, Guid answerId, Guid userId);

        Task DeleteAnswerAsync(Guid answerId, Guid userId);

        public void ValidateAnswerForQuestion(AnswerViewModel model, 
            Infrastructure.Data.Models.Question question, 
            IEnumerable<Infrastructure.Data.Models.Answer> otherAnswers);
    }
}
