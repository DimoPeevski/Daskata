using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Represents user roles in the app")]
    public class UserRole : IdentityRole<Guid>
    {
        [MaxLength(RoleLenghtMax)]
        [Comment("Name of the role translated in Bulgarian (e.g. Администратор, Мениджър, Учител, Учиник)")]
        public string? BGName { get; set; }
    }
}
