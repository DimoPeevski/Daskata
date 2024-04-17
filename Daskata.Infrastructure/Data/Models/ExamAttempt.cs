using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Exam attempted by user")]
    public class ExamAttempt
    {
        [Key]
        [Comment("Unique identifier for the exam attempt")]
        public Guid Id { get; set; }

        [Comment("Start time of the exam attempt")]
        public DateTime StartTime { get; set; }

        [Comment("End time of the exam attempt")]
        public DateTime EndTime { get; set; }

        [Comment("Duration of the exam attempt in minutes")]
        public TimeSpan DurationTaken { get; set; }

        [Comment("Indicates if the exam attempt is completed")]
        public bool IsCompleted { get; set; }

        [Comment("Score obtained in the exam attempt")]
        public int Score { get; set; }

        [Required]
        [Comment("Foreign key referencing the exam attempted")]
        public Guid ExamID { get; set; }

        [Required]
        [Comment("Foreign key referencing the user who attempted the exam")]
        public Guid UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        [Comment("Reference to the user who attempted the exam")]
        public virtual UserProfile User { get; set; } = null!;

        [ForeignKey(nameof(ExamID))]
        [Comment("Reference to the exam attempted")]
        public virtual Exam Exam { get; set; } = null!;
    }
}
