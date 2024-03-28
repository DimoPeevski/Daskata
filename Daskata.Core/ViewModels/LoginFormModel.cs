using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class LoginFormModel
    {
        [Required]
        [StringLength(UsernameLenghtMax, MinimumLength = UsernameLenghtMin,
            ErrorMessage = "{0}то трябва да бъде между {2} и {1} символа.")]
        [Display(Name = "Потребителско име")]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(PasswordLenghtMax, MinimumLength = PasswordLenghtMin,
            ErrorMessage = "{0}та трябва да бъде между {2} и {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }
}
