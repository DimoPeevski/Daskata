using System.ComponentModel.DataAnnotations;

namespace Daskata.Core.ViewModels
{
    public class ExamAttemptViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = " ")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = " ")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = " ")]
        public TimeSpan DurationTaken { get; set; }

        [Required]
        [Display(Name = " ")]
        public bool IsCompleted { get; set; }

        [Required]
        [Display(Name = " ")]
        public double Score { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ExamId { get; set; }
    }
}
