using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Represents a custom user role in the application")]
    public class UserRole : IdentityRole<Guid>
    {
        [Comment("Gets or sets the localized name of the role in Bulgarian")]
        [MaxLength(UserRoleBGNameLenghtMax)]
        public string? BGName { get; set; }
    }
}
