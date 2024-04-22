using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Constants.DataConstants;

namespace Daskata.Core.ViewModels
{
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(PasswordLenghtMax, MinimumLength = PasswordLenghtMin,
           ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = " ")]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(PasswordLenghtMax, MinimumLength = PasswordLenghtMin,
           ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = " ")]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = " ")]
        [Compare("NewPassword", ErrorMessage = "Въведените пароли не съвпадат")]
        public string ConfirmNewPassword { get; set; } = null!;

        [DataType(DataType.ImageUrl)]
        public string? ProfilePictureUrl { get; set; }
    }
}
