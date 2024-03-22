using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("User with status - Student")]
    public class Student
    {
        [Key]
        [Comment("Unique identifier for the student")]
        public int StudentID { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated user")]
        public required int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        [Comment("Reference to the associated user")]
        public virtual User User { get; set; } = null!;
    }
}