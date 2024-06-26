﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Daskata.Infrastructure.Constants.DataConstants;

namespace Daskata.Infrastructure.Data.Models
{
    [Comment("Represents individual users within the system")]
    public class UserProfile : IdentityUser<Guid>
    {
        [MaxLength(FirstNameLenghtMax)]
        [Comment("First name of the user")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(LastNameLenghtMax)]
        [Comment("Last name of the user")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Comment("Date and time when the user account was registered")]
        public DateTime RegistrationDate { get; set; }

        [Comment("Date and time of the user's last login")]
        public DateTime LastLoginDate { get; set; }

        [Comment("Indicates whether the user account is active or deactivated")]
        public bool IsActive { get; set; }

        [MaxLength(SchoolLenghtMax)]
        [Comment("Information about user school")]
        public string? School { get; set; } = string.Empty;

        [MaxLength(LocationLenghtMax)]
        [Comment("Information about user location")]
        public string? Location { get; set; } = string.Empty;

        [MaxLength(AdditionalInfoLenghtMax)]
        [Comment("Additional information about the user")]
        public string? AdditionalInfo { get; set; } = string.Empty;

        [Comment("Property to store the user ID of the creator")]
        public Guid? CreatedByUserId { get; set; }

        [Comment("URL for the user's profile picture")]
        public string ProfilePictureUrl { get; set; } = string.Empty;

        [Comment("Navigation property to refer to the user who created this profile")]
        public virtual UserProfile? CreatedByUser { get; set; }

        [Comment("Navigation property for associated exam attempts")]
        public virtual ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();

        [Comment("Navigation property for user responses to questions")]
        public virtual ICollection<UserExamResponse> UserExamResponses { get; set; } = new List<UserExamResponse>();
    }
}
