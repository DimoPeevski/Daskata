using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Core.ViewModels
{
    public class EditUserProfileInfoModel
    {
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

        [MaxLength(PhoneNumberLenghtMax)]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(AdditionalInfoLenghtMax)]
        [Phone]
        public string AdditionalInfo { get; set; } = null!;

        [DataType(DataType.ImageUrl)]
        public string ProfilePictureUrl { get; set; } = null!;

        public string RegistrationDate { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string School { get; set; } = null!;
    }
}
