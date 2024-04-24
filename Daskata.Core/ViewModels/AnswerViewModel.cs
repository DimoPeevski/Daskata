using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Constants.DataConstants;

namespace Daskata.Core.ViewModels
{
    public class AnswerViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(AnswerTextLenghtMax, MinimumLength = AnswerTextLenghtMin,
           ErrorMessage = "Отговорът да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string AnswerText { get; set; } = string.Empty;

        [Required]
        [Display(Name = " ")]
        public bool IsCorrect { get; set; }

        [Required]
        [Display(Name = " ")]
        public Guid QuestionId { get; set; }

        [Display(Name = " ")]
        public string? ParentExamUrl { get; set; } = string.Empty;
    }
}
