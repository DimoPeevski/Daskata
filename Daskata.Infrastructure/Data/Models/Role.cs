using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Represents user roles in the app")]
    public class Role
    {
        [Key]
        [Comment("Unique identifier for the role")]
        public Guid RoleId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(RoleLenghtMax)]
        [Comment("Name of the role (e.g. Admin, Manager, Teacher, Student)")]
        public string RoleName { get; set; } = string.Empty;
    }
}
