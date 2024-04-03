using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class EditUserFormModel
    {
        [Required]
        [StringLength(FirstNameLenghtMax, MinimumLength = FirstNameLenghtMin,
          ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [Display(Name = "Името")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameLenghtMax, MinimumLength = LastNameLenghtMin,
            ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [Display(Name = "Фамилията")]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(UsernameLenghtMax, MinimumLength = UsernameLenghtMin,
         ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [Display(Name = "Потребителското име")]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(EmailLenghtMax, MinimumLength = EmailLenghtMin,
            ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [StringLength(PhoneNumberLenghtMax, MinimumLength = PhoneNumberLenghtMin,
            ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [Display(Name = "Телефонът")]
        public string? PhoneNumber { get; set; }

        [StringLength(SchoolLenghtMax, MinimumLength = SchoolLenghtMin,
          ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [Display(Name = "Учебното заведение")]
        public string? School { get; set; }

        [StringLength(LocationLenghtMax, MinimumLength = LocationLenghtMin,
            ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [Display(Name = "Местонахождението")]
        public string? Location { get; set; }

        [StringLength(AdditionalInfoLenghtMax, MinimumLength = AdditionalInfoLenghtMin,
          ErrorMessage = "{0} да бъде между {2} и {1} символа.")]
        [Display(Name = "Допълнителната информация")]
        public string? AdditionalInfo { get; set; }

        [Display(Name = "Датата на регистрация")]
        public string RegistrationDate { get; set; } = null!;

        [DataType(DataType.ImageUrl)]
        public string? ProfilePictureUrl { get; set; }
    }
}
