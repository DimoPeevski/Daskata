using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Question in an exam")]
    public class Question
    {
        [Key]
        [Comment("Unique identifier for the question")]
        public int QuestionID { get; set; }

        [Comment("Text of the question")]
        public string QuestionText { get; set; } = string.Empty;

        [Comment("Type of the question (e.g., multiple choice, true/false)")]
        public string QuestionType { get; set; } = string.Empty;

        [Comment("Explanation or additional information for the question")]
        public string Explanation { get; set; } = string.Empty;

        [Comment("Points assigned to the question")]
        public int Points { get; set; }

        [Comment("Indicates if multiple correct answers are allowed")]
        public bool IsMultipleCorrect { get; set; }

        [Comment("Order index for sorting questions within an exam")]
        public int OrderIndex { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated exam")]
        public int ExamID { get; set; }

        [ForeignKey(nameof(ExamID))]
        [Comment("Reference to the associated exam")]
        public virtual Exam Exam { get; set; } = null!;
    }
}
