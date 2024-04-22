using Daskata.Core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Daskata.Core.Contracts.Profile
{
    public interface IProfileService
    {
        Task<UserProfileModel> GetUserProfileModelAsync(Guid userId);

        Task<UserProfileModel> GetUserPreviewModelAsync(string username);

        Task<EditUserFormModel> GetEditUserFormModelAsync(Guid userId);

        Task<IdentityResult> UpdateUserAsync(Guid userId, EditUserFormModel model);

        Task<bool> UsernameExistsAsync(string username);

        Task<bool> EmailExistsAsync(string email);

        Task<bool> PhoneNumberExistsAsync(string phoneNumber);

        Task<IdentityResult> ChangeUserPasswordAsync(Guid userId, string oldPassword, string newPassword);
    }
}
