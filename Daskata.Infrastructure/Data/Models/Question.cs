using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Question in an exam")]
    public class Question
    {
        [Key]
        [Comment("Unique identifier for the question")]
        public Guid Id { get; set; }

        [Required]
        [Comment("Text of the question")]
        [MaxLength(QuestionTextLenghtMax)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        [Comment("Type of the question (e.g., multiple choice, true/false)")]
        public string QuestionType { get; set; } = null!;

        [Required]
        [Comment("Indicates if multiple correct answers are allowed")]
        public bool IsMultipleCorrect { get; set; }

        [Comment("Explanation or additional information for the question")]
        public string? Explanation { get; set; } = string.Empty;

        [Required]
        [MaxLength(QuestionPointsMax)]
        [Comment("Points assigned to the question")]
        public int Points { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated exam")]
        public Guid ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        [Comment("Reference to the associated exam")]
        public virtual Exam Exam { get; set; } = null!;

        [Comment("List of answers to the question")]
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }

}
