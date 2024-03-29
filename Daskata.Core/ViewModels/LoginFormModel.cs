using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class LoginFormModel
    {
        [Required]
        [StringLength(UsernameLenghtMax, MinimumLength = UsernameLenghtMin,
            ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [Display(Name = "Потребителското име")]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(PasswordLenghtMax, MinimumLength = PasswordLenghtMin,
            ErrorMessage = "{0} трябва да е между {2} и {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = "Паролата")]
        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }
}
