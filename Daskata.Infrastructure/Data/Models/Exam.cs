﻿using Microsoft.EntityFrameworkCore;
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
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ExamTitleLenghtMax)]
        [Comment("Title of the exam")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(ExamDescriptionLenghtMax)]
        [Comment("Description of the exam")]
        public string? Description { get; set; }

        [Comment("Duration of the exam in minutes")]
        public TimeSpan Duration { get; set; }

        [Comment("Total points available in the exam")]
        public int TotalPoints { get; set; }

        [Comment("Indicates if the exam is published and available for students")]
        public bool IsPublished { get; set; } = false;

        [Required]
        [Comment("Date and time when the exam was created")]
        public DateTime CreationDate { get; set; }

        [Comment("Date and time when the exam was last modified")]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        [Comment("Unique URL for the exam")]
        public string ExamUrl { get; set; } = null!;

        [Required]
        [Comment("Foreign key referencing the user who created the exam")]
        public Guid CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        [Comment("Reference to the user who created the exam")]
        public virtual UserProfile User { get; set; } = null!;

        [Comment("Navigation property for questions in the exam")]
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
