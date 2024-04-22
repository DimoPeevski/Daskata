using Daskata.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Daskata.Infrastructure.Constants.DataConstants;

namespace Daskata.Core.ViewModels
{
    public class QuestionViewModel
    {
        [Key]
        [Display(Name = " ")]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = " ")]
        [StringLength(QuestionTextLenghtMax, MinimumLength = QuestionTextLenghtMin,
           ErrorMessage = "Въпросът да бъде между {2} и {1} символа.")]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        [Display(Name = " ")]
        [StringLength(QuestionTypeLenghtMax, MinimumLength = QuestionTypeLenghtMin,
           ErrorMessage = "Типът да бъде между {2} и {1} символа.")]
        public string QuestionType { get; set; } = null!;

        [Required]
        [Display(Name = " ")]
        public bool IsMultipleCorrect { get; set; }

        [Required]
        [Display(Name = " ")]
        [Range(QuestionPointsMin, QuestionPointsMax, 
            ErrorMessage = "Точките да са в интервал между {2} и {1}.")]
        public int Points { get; set; }

        [Required]
        [Display(Name = " ")]
        public Guid ExamId { get; set; }

        [Display(Name = " ")]
        [StringLength(QuestionExplanationLenghtMax, MinimumLength = QuestionExplanationLenghtMin,
           ErrorMessage = "Типът да бъде между {2} и {1} символа.")]
        public string? Explanation { get; set; } = string.Empty;

        [Display(Name = " ")]
        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
    }
}
