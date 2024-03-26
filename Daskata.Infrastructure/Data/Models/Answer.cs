using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Question answer")]
    public class Answer
    {
        [Key]
        [Comment("Unique identifier for the answer")]
        public Guid AnswerID { get; set; } = Guid.NewGuid();

        [Comment("Text of the answer")]
        public string AnswerText { get; set; } = string.Empty;

        [Comment("Indicates if the answer is correct")]
        public bool IsCorrect { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated question")]
        public Guid QuestionID { get; set; }

        [ForeignKey(nameof(QuestionID))]
        [Comment("Reference to the associated question")]
        public virtual Question Question { get; set; } = null!;
    }
}
