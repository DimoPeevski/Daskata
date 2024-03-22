﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Exam attempted by user")]
    public class ExamAttempt
    {
        [Key]
        [Comment("Unique identifier for the exam attempt")]
        public int AttemptID { get; set; }

        [Comment("Start time of the exam attempt")]
        public DateTime StartTime { get; set; }

        [Comment("End time of the exam attempt")]
        public DateTime EndTime { get; set; }

        [Comment("Duration of the exam attempt in minutes")]
        public int DurationTaken { get; set; }

        [Comment("Indicates if the exam attempt is completed")]
        public bool IsCompleted { get; set; }

        [Comment("Score obtained in the exam attempt")]
        public int Score { get; set; }

        [Comment("Foreign key referencing the user who attempted the exam")]
        [ForeignKey(nameof(Models.User))]
        public int UserID { get; set; }

        [Comment("Foreign key referencing the exam attempted")]
        [ForeignKey(nameof(Models.Exam))]
        public int ExamID { get; set; }

        [Comment("Reference to the user who attempted the exam")]
        public virtual User User { get; set; } = null!;

        [Comment("Reference to the exam attempted")]
        public virtual Exam Exam { get; set; } = null!;
    }
}
