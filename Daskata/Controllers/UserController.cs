using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Daskata.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly IUserStore<UserProfile> _userStore;
        private readonly ILogger<LoginFormModel> _logger;
        private readonly DaskataDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(SignInManager<UserProfile> signInManager,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context,
                              ILogger<LoginFormModel> logger,
                              IUserStore<UserProfile> userStore,
                              IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _userStore = userStore;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginFormModel model = new LoginFormModel();
            {
                model.ReturnUrl = returnUrl;
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Някъде има грешка...");

                return View(model);
            }

            var curentUserId = GetCurentUserId();
            _logger.LogInformation($"User with id {curentUserId} logged in.");

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            string uniqueUsername = await GenerateUniqueUsernameAsync();

            if (uniqueUsername == string.Empty)
            {
                return RedirectToAction("Index", "Home");
            }

            UserProfile user = new UserProfile()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = model.Role,
                RegistrationDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                IsActive = true,
                EmailConfirmed = false,
                PhoneNumber = string.Empty,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
            };

            user.CreatedByUserId = await GetCurentUserId();
            string serviceGeneratedEmail = $"{uniqueUsername}@daskata.eu";

            await _userManager.SetUserNameAsync(user, uniqueUsername);
            await _userManager.SetEmailAsync(user, serviceGeneratedEmail);
            
            var result = await _userManager.CreateAsync(user, model.Password);
            _logger.LogInformation
                ($"New user with id {uniqueUsername} was created successfully by user with id {user.CreatedByUserId}.");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        private async Task<string> GenerateUniqueUsernameAsync()
        {
            string username = string.Empty;
            bool usernameExists = true;
            int counter = 0;

            while (usernameExists)
            {
                Random rand = new Random();
                string randomNumbers = rand.Next(100000, 999999).ToString();
                username = $"user{randomNumbers}";

                usernameExists = await _context.UserProfiles.AnyAsync(u => u.UserName == username);
                counter++;

                if (counter > 999999)
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
