using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data.Models;

namespace Daskata.Core.Contracts.Network
{
    public interface INetworkService
    {
        Task<List<UserProfileModel>> GetConnectedUsersAsync(UserProfile user);

        Task<List<UserProfileModel>> GetUsersCreatedByAsync(Guid userId);

        Task<EditUserFormModel> GetUserForEditAsync(string username);

        Task EditUserAsync(string currentUsername, EditUserFormModel editModel);

        Task<UserProfileModel> GetUserProfileForDeletionAsync(string username);

        Task DeactivateUserAsync(string username);

        Task<bool> UsernameExistsAsync(string username);

        Task<bool> EmailExistsAsync(string email);

        Task<bool> PhoneNumberExistsAsync(string phoneNumber);

    }
}
