using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data.Models;

namespace Daskata.Core.Contracts.Exam
{
    public interface IExamService
    {
        Task<List<FullExamViewModel>> GetAllExamsAsync();

        Task<List<FullExamViewModel>> GetExamsByCreatorAsync(Guid userId);

        Task<FullExamViewModel> GetExamPreview(string examUrl);

        Task<FullExamViewModel> GetOpenExam(string examUrl);

        Task<List<string>> Create();

        Task<List<string>> Grade(string subject);

        Task<FullExamViewModel> Details(string grade);

        Task<Infrastructure.Data.Models.Exam> Publish(FullExamViewModel model, string subject, string grade, Guid userId);

        Task<Infrastructure.Data.Models.Exam> GetExamByUrlAsync(string examUrl);

        Task<Infrastructure.Data.Models.Exam> UpdateExamAsync(FullExamViewModel model);

        Task<FullExamViewModel> GetExamAndQuestionsByUrlAsync(string examUrl);

        Task<ExamAttempt> CalculateExamResultAsync(string examUrl, ExamAttemptViewModel model);

        double CalculateTotalScoreInPercentage(List<QuestionViewModel> questions, int maxPossiblePoints);

    }
}
