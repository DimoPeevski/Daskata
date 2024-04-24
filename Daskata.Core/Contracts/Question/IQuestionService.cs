using Daskata.Core.ViewModels;

namespace Daskata.Core.Contracts.Question
{
    public interface IQuestionService
    {
        Task<QuestionViewModel> PrepareQuestionForCreationAsync(string parentExamUrl);

        Task<Guid> CreateQuestionAsync(QuestionViewModel model, string examUrl);

        Task<QuestionViewModel> GetQuestionForEditAsync(Guid questionId, Guid userId);

        Task<bool> UpdateQuestionAsync(QuestionViewModel model, string examUrl);

        Task<bool> DeleteQuestionAsync(Guid questionId, Guid userId);
    }
}
