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

        [ForeignKey(nameof(User))]
        [Comment("Foreign key referencing the associated user")]
        public int UserID { get; set; }
    }
}