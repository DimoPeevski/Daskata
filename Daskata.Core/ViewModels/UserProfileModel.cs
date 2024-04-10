using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class UserProfileModel
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
        [StringLength(UsernameLenghtMax, MinimumLength = UsernameLenghtMin,
         ErrorMessage = "Потребителското име да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(EmailLenghtMax, MinimumLength = EmailLenghtMin,
            ErrorMessage = "Email да бъде между {2} и {1} символа.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [StringLength(PhoneNumberLenghtMax, MinimumLength = PhoneNumberLenghtMin,
            ErrorMessage = "Телефонът да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string? PhoneNumber { get; set; }

        [StringLength(SchoolLenghtMax, MinimumLength = SchoolLenghtMin,
          ErrorMessage = "Учебното заведение да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string? School { get; set; }

        [StringLength(LocationLenghtMax, MinimumLength = LocationLenghtMin,
            ErrorMessage = "Местонахождението да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string? Location { get; set; }

        [StringLength(AdditionalInfoLenghtMax, MinimumLength = AdditionalInfoLenghtMin,
          ErrorMessage = "Допълнителната информация да бъде между {2} и {1} символа.")]
        [Display(Name = " ")]
        public string? AdditionalInfo { get; set; }

        [Display(Name = "Датата на регистрация")]
        public string RegistrationDate { get; set; } = null!;

        [DataType(DataType.ImageUrl)]
        public string? ProfilePictureUrl { get; set; }

        public List<PartialExamViewModel>? Exams { get; set; }

        public bool IsActive { get; set; }
    }
}
