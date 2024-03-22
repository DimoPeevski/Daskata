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

        [ForeignKey(nameof(User))]
        [Comment("Foreign key referencing the associated user")]
        public int UserID { get; set; }
    }
}