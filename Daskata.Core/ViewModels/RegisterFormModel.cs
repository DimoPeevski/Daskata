using Daskata.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class RegisterFormModel
    {
        [Required]
        [StringLength(FirstNameLenghtMax, MinimumLength = FirstNameLenghtMin,
            ErrorMessage = "{0}то трябва да бъде между {2} и {1} символа.")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameLenghtMax, MinimumLength = LastNameLenghtMin,
            ErrorMessage = "{0}та трябва да бъде между {2} и {1} символа.")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(PasswordLenghtMax, MinimumLength = PasswordLenghtMin, 
            ErrorMessage = "{0}та трябва да бъде между {2} и {1} символа.")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Повтори парола")]
        [Compare("Password", ErrorMessage = "Въведените пароли не съвпадат")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public RoleCategory Role { get; set; }
    }
}
