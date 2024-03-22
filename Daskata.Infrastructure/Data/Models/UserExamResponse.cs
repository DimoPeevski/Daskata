using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Records user responses to questions")]
    public class UserExamResponse
    {
        [Key]
        [Comment("Unique identifier for the user's exam response")]
        public Guid ResponseID { get; set; }

        [Comment("Indicates if the user's response is correct")]
        public bool IsCorrect { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated question")]
        public Guid QuestionID { get; set; }

        [Required]
        [Comment("Foreign key referencing the selected answer")]
        public Guid AnswerID { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated exam attempt")]
        public Guid AttemptID { get; set; }

        [ForeignKey(nameof(QuestionID))]
        [Comment("Reference to the associated question")]
        public virtual Question Question { get; set; } = null!;

        [ForeignKey(nameof(AnswerID))]
        [Comment("Reference to the selected answer")]
        public virtual Answer Answer { get; set; } = null!;

        [ForeignKey(nameof(AttemptID))]
        [Comment("Reference to the associated exam attempt")]
        public virtual ExamAttempt ExamAttempt { get; set; } = null!;
    }
}
