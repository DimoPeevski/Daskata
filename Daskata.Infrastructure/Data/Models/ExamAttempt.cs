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

        [Required]
        [Comment("Start time of the exam attempt")]
        public DateTime StartTime { get; set; }

        [Required]
        [Comment("End time of the exam attempt")]
        public DateTime EndTime { get; set; }

        [Required]
        [Comment("Duration of the exam attempt in minutes")]
        public TimeSpan DurationTaken { get; set; }

        [Required]
        [Comment("Indicates if the exam attempt is completed")]
        public bool IsCompleted { get; set; }

        [Required]
        [Comment("Score in percentages (Score/100) obtained in the exam attempt")]
        public double Score { get; set; }

        [Required]
        [Comment("Foreign key referencing the user who attempted the exam")]
        public Guid UserId { get; set; }

        [Required]
        [Comment("Foreign key referencing the exam attempted")]
        public Guid ExamId { get; set; }

        [ForeignKey(nameof(UserId))]
        [Comment("Reference to the user who attempted the exam")]
        public virtual UserProfile User { get; set; } = null!;

        [ForeignKey(nameof(ExamId))]
        [Comment("Reference to the exam attempted")]
        public virtual Exam Exam { get; set; } = null!;
    }
}
