using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Constants.DataConstants;

namespace Daskata.Core.ViewModels
{
    public class LoginUserFormModel
    {
        [Required]
        [StringLength(UsernameLenghtMax, MinimumLength = UsernameLenghtMin,
            ErrorMessage = "Потребителското име да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(PasswordLenghtMax, MinimumLength = PasswordLenghtMin,
            ErrorMessage = "Паролата трябва да е между {2} и {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = " ")]
        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }
}
