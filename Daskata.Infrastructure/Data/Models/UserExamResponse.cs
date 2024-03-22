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
        public int ResponseID { get; set; }

        [Comment("Indicates if the user's response is correct")]
        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(Models.ExamAttempt))]
        [Comment("Foreign key referencing the associated exam attempt")]
        public int AttemptID { get; set; }

        [ForeignKey(nameof(Models.Question))]
        [Comment("Foreign key referencing the associated question")]
        public int QuestionID { get; set; }

        [ForeignKey(nameof(Models.Answer))]
        [Comment("Foreign key referencing the selected answer")]
        public int SelectedAnswerID { get; set; }

        [Comment("Reference to the associated exam attempt")]
        public virtual ExamAttempt ExamAttempt { get; set; } = null!;

        [Comment("Reference to the associated question")]
        public virtual Question Question { get; set; } = null!;

        [Comment("Reference to the selected answer")]
        public virtual Answer Answer { get; set; } = null!;
    }
}
