using Daskata.Core.Contracts.Network;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Daskata.Core.Shared.Methods;

namespace Daskata.Core.Services.Network
{
    public class NetworkService : INetworkService
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly IRepository _repository;

        public NetworkService(IRepository repository, UserManager<UserProfile> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<List<UserProfileModel>> GetConnectedUsersAsync(UserProfile user)
        {
            if (user == null || !user.IsActive)
            {
                throw new InvalidOperationException("Потребителят не беше намерен или не е активен.");
            }

            var userConnections = await _repository.All<UserConnection>()
                .Include(uc => uc.FirstUser)
                .Include(uc => uc.SecondUser)
                .ToListAsync();

            var connectedUsers = new List<UserProfile>();
            foreach (var connection in userConnections)
            {
                if (!connectedUsers.Contains(connection.FirstUser))
                {
                    connectedUsers.Add(connection.FirstUser);
                }
                if (!connectedUsers.Contains(connection.SecondUser))
                {
                    connectedUsers.Add(connection.SecondUser);
                }
            }

            return connectedUsers.Select(u => new UserProfileModel
            {
                ProfilePictureUrl = u.ProfilePictureUrl,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.UserName!,
                AdditionalInfo = u.AdditionalInfo,
                School = u.School,
                RegistrationDate = GetRegistrationMonthYearAsNumbers(u.RegistrationDate.ToString()),
                Location = u.Location,
                IsActive = u.IsActive,
            }).ToList();
        }

        public async Task<List<UserProfileModel>> GetUsersCreatedByAsync(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "Потребителското ID не може да бъде празно.");
            }

            var usersCreatedByMe = await _repository.All<UserProfile>()
                .Where(u => u.CreatedByUserId == userId)
                .ToListAsync();

            return usersCreatedByMe.Select(u => new UserProfileModel
            {
                ProfilePictureUrl = u.ProfilePictureUrl,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.UserName!,
                AdditionalInfo = u.AdditionalInfo,
                School = u.School,
                RegistrationDate = GetRegistrationMonthYearAsNumbers(u.RegistrationDate.ToString()),
                Location = u.Location,
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task<EditUserFormModel> GetUserForEditAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Потребителското ID не може да бъде празно.", nameof(username));
            }

            var userToEdit = await _repository.All<UserProfile>()
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (userToEdit == null || !userToEdit.IsActive)
            {
                throw new InvalidOperationException("Потребителят не беше намерен или не е активен.");
            }

            return new EditUserFormModel
            {
                Username = userToEdit.UserName,
                Email = userToEdit.Email,
                FirstName = userToEdit.FirstName,
                LastName = userToEdit.LastName,
                PhoneNumber = userToEdit.PhoneNumber,
                AdditionalInfo = userToEdit.AdditionalInfo,
                ProfilePictureUrl = userToEdit.ProfilePictureUrl,
                Location = userToEdit.Location,
                School = userToEdit.School
            };
        }

        public async Task EditUserAsync(string currentUsername, EditUserFormModel editModel)
        {

            if (string.IsNullOrEmpty(currentUsername))
            {
                throw new ArgumentException("Потребителското име на логнатият потребител е задължително.", nameof(currentUsername));
            }

            if (editModel == null)
            {
                throw new ArgumentNullException(nameof(editModel), "Моделът, по който се правят корекциите не може да е празен.");
            }

            var userToUpdate = await _repository.All<UserProfile>()
                .FirstOrDefaultAsync(u => u.UserName == currentUsername);

            if (userToUpdate == null)
            {
                throw new InvalidOperationException("Потребителят не е намерен.");
            }

            userToUpdate.FirstName = editModel.FirstName;
            userToUpdate.LastName = editModel.LastName;
            userToUpdate.ProfilePictureUrl = editModel.ProfilePictureUrl;

            userToUpdate.School = editModel.School ?? string.Empty;
            userToUpdate.Location = editModel.Location ?? string.Empty;
            userToUpdate.AdditionalInfo = editModel.AdditionalInfo ?? string.Empty;
            userToUpdate.PhoneNumber = editModel.PhoneNumber ?? string.Empty;

            if (userToUpdate.UserName != editModel.Username)
            {
                var usernameExists = await UsernameExistsAsync(editModel.Username);
                if (usernameExists)
                {
                    throw new InvalidOperationException("Потребителското име вече съществува.");
                }
            }

            userToUpdate.UserName = editModel.Username;

            if (userToUpdate.Email != editModel.Email)
            {
                var emailExists = await EmailExistsAsync(editModel.Email);
                if (emailExists && editModel.Email != "no@email.xyz" && !string.IsNullOrWhiteSpace(editModel.Email))
                {
                    throw new InvalidOperationException("E-mail адресът име вече съществува.");
                }
            }

            userToUpdate.Email = editModel.Email.ToLower();

            if (userToUpdate.PhoneNumber != editModel.PhoneNumber && !string.IsNullOrWhiteSpace(editModel.PhoneNumber))
            {
                var phoneNumberExists = await PhoneNumberExistsAsync(editModel.PhoneNumber);
                if (phoneNumberExists)
                {
                    throw new InvalidOperationException("Телефонът вече съществува.");
                }
            }

            userToUpdate.PhoneNumber = editModel.PhoneNumber;

            var result = await _userManager.UpdateAsync(userToUpdate);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Не успяхме да актуализираме потребителя.");
            }

            await _repository.SaveChangesAsync();
        }

        public async Task<UserProfileModel> GetUserProfileForDeletionAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Потребителят не може да е празна стойност.", nameof(username));
            }

            var userToDelete = await _repository.All<UserProfile>()
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (userToDelete == null || !userToDelete.IsActive)
            {
                throw new InvalidOperationException("Потребителят не е намерен или не е активен.");
            }

            return new UserProfileModel
            {
                FirstName = userToDelete.FirstName,
                LastName = userToDelete.LastName,
                Username = userToDelete.UserName!,
                ProfilePictureUrl = userToDelete.ProfilePictureUrl,
                School = userToDelete.School,
                Location = userToDelete.Location,
                PhoneNumber = userToDelete.PhoneNumber,
                RegistrationDate = userToDelete.RegistrationDate.ToString(),
                Email = userToDelete.Email!
            };
        }

        public async Task DeactivateUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Потребителят не може да има нулева стойност.", nameof(username));
            }

            var userToDeactivate = await _repository.All<UserProfile>()
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (userToDeactivate == null)
            {
                throw new InvalidOperationException("Потребителят не беше намерен.");
            }

            userToDeactivate.ProfilePictureUrl = "/lib/profile-pictures/default-profile-image.png";
            userToDeactivate.AdditionalInfo = string.Empty;
            userToDeactivate.School = string.Empty;
            userToDeactivate.Location = string.Empty;
            userToDeactivate.PhoneNumber = string.Empty;
            userToDeactivate.Email = "no@email.xyz";
            userToDeactivate.IsActive = false;

            var result = await _userManager.UpdateAsync(userToDeactivate);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Не успяхме да деактивираме потребителя.");
            }
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            var context = _repository.All<UserProfile>();
            return await context.AnyAsync(u => u.UserName == username);
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
    }
}
