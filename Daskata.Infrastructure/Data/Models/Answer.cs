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
        public int AnswerID { get; set; }

        [Comment("Text of the answer")]
        public string AnswerText { get; set; } = string.Empty;

        [Comment("Indicates if the answer is correct")]
        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(Models.Question))]
        [Comment("Foreign key referencing the associated question")]
        public int QuestionID { get; set; }

        [Comment("Reference to the associated question")]
        public virtual Question Question { get; set; } = null!;
    }

}
