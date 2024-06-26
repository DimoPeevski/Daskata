﻿using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Constants.DataConstants;

namespace Daskata.Core.ViewModels
{
    public class PartialExamViewModel
    {
        [Required]
        [StringLength(ExamTitleLenghtMax, MinimumLength = ExamTitleLenghtMin,
          ErrorMessage = "Заглавието да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string Title { get; set; } = null!;

        [StringLength(ExamDescriptionLenghtMax, MinimumLength = ExamDescriptionLenghtMin,
        ErrorMessage = "Краткото описание да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = " ")]
        public TimeSpan Duration { get; set; }

        [Required]
        [Display(Name = " ")]
        public int TotalPoints { get; set; }

        [Required]
        [Display(Name = " ")]
        public bool IsPublished { get; set; } = false;

        [Required]
        [Display(Name = " ")]
        public DateTime CreationDate { get; set; }

        [Display(Name = " ")]
        public string ExamUrl { get; set; } = null!;

        [Display(Name = " ")]
        public Guid CreatedByUserId { get; set; }
    }
}
