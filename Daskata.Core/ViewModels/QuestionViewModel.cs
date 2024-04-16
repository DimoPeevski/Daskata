using Daskata.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string QuestionType { get; set; } = null!;
        public bool IsMultipleCorrect { get; set; }
        public int Points { get; set; }
        public Guid ExamId { get; set; }
        public string? Explanation { get; set; } = string.Empty;
        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
    }
}
