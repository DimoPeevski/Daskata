using Daskata.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class FullExamViewModel
    {
        [Key]
        public Guid Id { get; set; }

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
        public int Duration { get; set; }

        [Required]
        [Display(Name = " ")]
        public int TotalPoints { get; set; }

        [Display(Name = " ")]
        public bool IsPublished { get; set; } = false;

        [Display(Name = " ")]
        public DateTime CreationDate { get; set; }

        [Display(Name = " ")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = " ")]
        public string? ExamUrl { get; set; }

        [Display(Name = " ")]
        public Guid CreatedByUserId { get; set; }

        [Display(Name = " ")]
        public bool IsPublic { get; set; } = false;

        [Display(Name = " ")]
        public SubjectCategory StudySubject { get; set; }

        [Display(Name = " ")]
        public GradeCategory StudentGrade { get; set; }

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
