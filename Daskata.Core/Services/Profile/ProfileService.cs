using Daskata.Core.Contracts.Profile;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Daskata.Core.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly ILogger<ProfileService> _logger;
        private readonly IRepository _repository;


        public ProfileService
            (UserManager<UserProfile> userManager,
            SignInManager<UserProfile> signInManager,
            ILogger<ProfileService> logger,
            IRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _repository = repository;
        }

        public async Task<UserProfileModel> GetUserProfileModelAsync(Guid userId)
        {
            var loggedUser = await _repository.All<UserProfile>()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (loggedUser == null)
            {
                return null!;
            }

            var model = new UserProfileModel
            {
                Username = loggedUser!.UserName!,
                Email = loggedUser.Email!,
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                PhoneNumber = loggedUser.PhoneNumber,
                AdditionalInfo = loggedUser.AdditionalInfo,
                ProfilePictureUrl = loggedUser.ProfilePictureUrl,
                School = loggedUser.School,
                RegistrationDate = loggedUser.RegistrationDate.ToString(),
                Location = loggedUser.Location,

                Exams = await _repository.All<Exam>()
               .Where(e => e.CreatedByUserId == loggedUser.Id)
               .Select(e => new FullExamViewModel
               {
                   Title = e.Title,
                   Description = e.Description,
                   Duration = (int)e.Duration.TotalMinutes,
                   TotalPoints = e.TotalPoints,
                   IsPublished = e.IsPublished,
                   CreationDate = e.CreationDate,
                   ExamUrl = e.ExamUrl,
                   CreatedByUserId = e.CreatedByUserId,
                   IsPublic = e.IsPublic,
               }).ToListAsync()
            };

            return model;
        }

        public async Task<UserProfileModel> GetUserPreviewModelAsync(string username)
        {
            var currentUser = await _userManager.FindByNameAsync(username);

            if (currentUser == null || !currentUser.IsActive)
            {

                return null!;
            }

            var model = new UserProfileModel
            {
                Username = currentUser!.UserName!,
                Email = currentUser.Email!,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhoneNumber = currentUser.PhoneNumber,
                AdditionalInfo = currentUser.AdditionalInfo,
                ProfilePictureUrl = currentUser.ProfilePictureUrl,
                School = currentUser.School,
                RegistrationDate = currentUser.RegistrationDate.ToString(),
                Location = currentUser.Location,

                Exams = await _repository.All<Exam>()
                .Where(e => e.CreatedByUserId == currentUser.Id)
                .Select(e => new FullExamViewModel
                {
                    Title = e.Title,
                    Description = e.Description,
                    Duration = (int)e.Duration.TotalMinutes,
                    TotalPoints = e.TotalPoints,
                    IsPublished = e.IsPublished,
                    CreationDate = e.CreationDate,
                    ExamUrl = e.ExamUrl,
                    CreatedByUserId = e.CreatedByUserId
                }).ToListAsync()
            };

            return model;
        }

        public async Task<EditUserFormModel> GetEditUserFormModelAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null || !user.IsActive)
            {
                return null;
            }

            var model = new EditUserFormModel
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AdditionalInfo = user.AdditionalInfo,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Location = user.Location,
                School = user.School
            };

            return model;
        }

        public async Task<IdentityResult> UpdateUserAsync(Guid userId, EditUserFormModel model)
        {
            var loggedUser = await _userManager.FindByIdAsync(userId.ToString());

            if (loggedUser == null || !loggedUser.IsActive)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Потребителят не беше намерен." });
            }

            loggedUser.FirstName = model.FirstName;
            loggedUser.LastName = model.LastName;
            loggedUser.School = model.School ?? string.Empty;
            loggedUser.Location = model.Location ?? string.Empty;
            loggedUser.AdditionalInfo = model.AdditionalInfo ?? string.Empty;
            loggedUser.PhoneNumber = model.PhoneNumber ?? string.Empty;

            if (loggedUser.UserName != model.Username)
            {
                if (await UsernameExistsAsync(model.Username))
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Потребителското име вече съществува." });
                }
                loggedUser.UserName = model.Username;
            }

            if (loggedUser.Email != model.Email)
            {
                if (await EmailExistsAsync(model.Email) && model.Email != "no@email.xyz" && model.Email != string.Empty)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Email адресът вече съществува." });
                }
                loggedUser.Email = model.Email.ToLowerInvariant();
            }

            if (loggedUser.PhoneNumber != model.PhoneNumber && model.PhoneNumber != string.Empty)
            {
                if (await PhoneNumberExistsAsync(model.PhoneNumber!))
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Телефонният номер вече съществува." });
                }
                loggedUser.PhoneNumber = model.PhoneNumber;
            }

            var result = await _userManager.UpdateAsync(loggedUser);
            if (!result.Succeeded)
            {
                _logger.LogError($"User with id {loggedUser.Id} could not be edited: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return result;
            }

            _logger.LogInformation($"User with id {loggedUser.Id} was edited.");

            return IdentityResult.Success;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            var allProfiles = _repository.All<UserProfile>();
            return await allProfiles.AnyAsync(u => u.UserName == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var context = _repository.All<UserProfile>();
            if (email.ToLower() == "no@email.xyz")
            {
                return false;
            }

            return await context.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            var context = _repository.All<UserProfile>();
            return await context.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<IdentityResult> ChangeUserPasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || !user.IsActive)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found or not active." });
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                return result;
            }

            await _signInManager.RefreshSignInAsync(user);

            return IdentityResult.Success;
        }

    }
}
