using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("User with status - Teacher")]
    public class Teacher
    {
        [Key]
        [Comment("Unique identifier for the teacher")]
        public int TeacherID { get; set; }

        [ForeignKey(nameof(User))]
        [Comment("Foreign key referencing the associated user")]
        public int UserID { get; set; }
    }
}