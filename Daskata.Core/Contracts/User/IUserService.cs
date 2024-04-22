using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Daskata.Core.Contracts.User
{
    public interface IUserService
    {
        Task<string> GenerateUniqueUsernameAsync();

        Task<IdentityResult> RegisterUserAsync(RegisterUserFormModel model, Guid createdByUserId);

        Task<(SignInResult signInResult, UserProfile userProfile)> LoginUserAsync(string userName, string password);

        Task LogoutUserAsync();

        Task<Guid?> GetCurentUserId();
    }
}
