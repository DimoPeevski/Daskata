using Daskata.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class RegisterUserFormModel
    {
        [Required]
        [StringLength(FirstNameLenghtMax, MinimumLength = FirstNameLenghtMin,
            ErrorMessage = "Името да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameLenghtMax, MinimumLength = LastNameLenghtMin,
            ErrorMessage = "Фамилията да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(PasswordLenghtMax, MinimumLength = PasswordLenghtMin, 
            ErrorMessage = "Паролата да бъде между {2} и {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = "")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Повторната парола")]
        [Compare("Password", ErrorMessage = "Въведените пароли не съвпадат")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public RoleCategory Role { get; set; }
    }
}
