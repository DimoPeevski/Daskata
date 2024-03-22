using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Exam to be passed")]
    public class Exam
    {
        [Key]
        [Comment("Unique identifier for the exam")]
        public int ExamID { get; set; }

        [Required]
        [Comment("Title of the exam")]
        [MaxLength(ExamTitleLenghtMax)]
        public required string Title { get; set; }

        [Comment("Description of the exam")]
        public string Description { get; set; } = string.Empty;

        [Comment("Duration of the exam in minutes")]
        public int DurationInMinutes { get; set; }

        [Comment("otal points available in the exam")]
        public int TotalPoints { get; set; }

        [Comment("Indicates if the exam is published and available for students")]
        public bool IsPublished { get; set; }

        [Required]
        [Comment("Date and time when the exam was created")]
        public required DateTime CreationDate { get; set; }

        [Comment("Date and time when the exam was last modified")]
        public DateTime LastModifiedDate { get; set; }

        [ForeignKey(nameof(Models.User))]
        [Comment("Foreign key referencing the user who created the exam")]
        public int UserID { get; set; }

        [Comment("Reference to the user who created the exa")]
        public virtual User User { get; set; } = null!;
    }
}
