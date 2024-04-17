using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Question answer")]
    public class Answer
    {
        [Key]
        [Comment("Unique identifier for the answer")]
        public Guid Id { get; set; }

        [Comment("Text of the answer")]
        [Required]
        [MaxLength(AnswerTextLenghtMax)]
        public string AnswerText { get; set; } = string.Empty;

        [Required]
        [Comment("Indicates if the answer is correct")]
        public bool IsCorrect { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated question")]
        public Guid QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        [Comment("Reference to the associated question")]
        public virtual Question Question { get; set; } = null!;
    }
}
