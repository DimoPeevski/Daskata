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
        public Guid QuestionID { get; set; } = Guid.NewGuid();

        [Comment("Text of the question")]
        public string QuestionText { get; set; } = string.Empty;

        [Comment("Type of the question (e.g., multiple choice, true/false)")]
        public string QuestionType { get; set; } = string.Empty;

        [Comment("Indicates if multiple correct answers are allowed")]
        public bool IsMultipleCorrect { get; set; }

        [Comment("Explanation or additional information for the question")]
        public string Explanation { get; set; } = string.Empty;

        [Comment("Points assigned to the question")]
        public int Points { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated exam")]
        public Guid ExamID { get; set; }

        [ForeignKey(nameof(ExamID))]
        [Comment("Reference to the associated exam")]
        public virtual Exam Exam { get; set; } = null!;

        [Comment("List of answers to the question")]
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }

}
