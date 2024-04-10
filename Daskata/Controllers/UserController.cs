using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<LoginUserFormModel> _logger;
        private readonly DaskataDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(SignInManager<UserProfile> signInManager,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context,
                              ILogger<LoginUserFormModel> logger,
                              IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginUserFormModel model = new();
            {
                model.ReturnUrl = returnUrl;
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);
            var loggedUser = await _userManager.GetUserAsync(User);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, signInErrorMessage);
                return View(model);
            }

            loggedUser!.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(loggedUser);

            var curentUserId = GetCurentUserId();
            _logger.LogInformation($"User with id {curentUserId} logged in.");

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string uniqueUsername = await GenerateUniqueUsernameAsync();

            if (uniqueUsername == string.Empty)
            {
                ModelState.AddModelError(string.Empty, uniqueUserGeneratedFailMessage);
                return RedirectToAction("Index", "Home");
            }

            UserProfile user = new()
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
                ProfilePictureUrl = "/lib/profile-pictures/default-profile-image.png"
            };

            string userEmail = "no@email.xyz";
            string userRole = model.Role.ToString();

            user.CreatedByUserId = await GetCurentUserId();
            await _userManager.SetUserNameAsync(user, uniqueUsername);
            await _userManager.SetEmailAsync(user, userEmail);

            var result = await _userManager.CreateAsync(user, model.Password);

            _logger.LogInformation
                ($"New user with id {uniqueUsername} was created successfully by user with id {user.CreatedByUserId}.");

            await _userManager.AddToRoleAsync(user, userRole);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return RedirectToAction("Index", "Home");
        }

        // Methods used in class: UserController

        private async Task<string> GenerateUniqueUsernameAsync()
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

                usernameExists = await _context.UserProfiles.AnyAsync(u => u.UserName == username)
                    || usernamesGenerated.Contains(username);
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

        private async Task<Guid?> GetCurentUserId()
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
    }
}
