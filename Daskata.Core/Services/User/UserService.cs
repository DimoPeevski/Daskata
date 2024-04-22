using Daskata.Core.Contracts.User;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Common;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Daskata.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository _repository;

        public UserService
            (UserManager<UserProfile> userManager,
            SignInManager<UserProfile> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        public async Task<(SignInResult signInResult, UserProfile userProfile)> LoginUserAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return (result, null);
            }

            var user = await _userManager.FindByNameAsync(userName);
            user!.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return (result, user);
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserFormModel model, Guid createdByUserId)
        {
            string uniqueUsername = await GenerateUniqueUsernameAsync();
            if (uniqueUsername == string.Empty)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Failed to generate unique username." });
            }

            UserProfile user = new UserProfile
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                RegistrationDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                IsActive = true,
                EmailConfirmed = false,
                PhoneNumber = string.Empty,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                ProfilePictureUrl = "/lib/profile-pictures/default-profile-image.png",
                CreatedByUserId = createdByUserId
            };

            string userEmail = "no@email.xyz";
            await _userManager.SetUserNameAsync(user, uniqueUsername);
            await _userManager.SetEmailAsync(user, userEmail);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            string userRole = model.Role.ToString();
            await _userManager.AddToRoleAsync(user, userRole);

            return IdentityResult.Success;
        }

        public async Task<Guid?> GetCurentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && Guid.TryParse(userIdClaim, out Guid parsedUserId))
            {
                return await Task.FromResult(parsedUserId);
            }
            else
            {
                return await Task.FromResult<Guid?>(null);
            }
        }

        public async Task<string> GenerateUniqueUsernameAsync()
        {
            string username = string.Empty;
            List<string> usernamesGenerated = new();
            bool usernameExists = true;
            int counter = 0;
            Random random = new();

            while (usernameExists)
            {
                string randomNumbers = random.Next(100000, 999999).ToString();
                username = $"user{randomNumbers}";

                usernameExists = await _repository
                    .AllReadonly<UserProfile>()
                    .AnyAsync(u => u.UserName == username
                || usernamesGenerated.Contains(username));

                counter++;

                if (usernameExists)
                {
                    usernamesGenerated.Add(username);
                }

                if (counter == 999_999)
                {
                    username = string.Empty;
                    break;
                }
            }

            return username;
        }
    }
}
