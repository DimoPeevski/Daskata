using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("App administator")]
    public class Admin
    {
        [Key]
        [Comment("Unique identifier for the admin")]
        public int AdminID { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated user")]
        public required int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        [Comment("Reference to the associated user")]
        public virtual User User { get; set; } = null!;
    }
}