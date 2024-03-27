using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly IUserStore<UserProfile> _userStore;
        private readonly ILogger<UserProfile> _logger;
        private readonly DaskataDbContext _context;

        public UserController(SignInManager<UserProfile> signInManager,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context, 
                              ILogger<UserProfile> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _logger = logger;
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
                Role = model.Role,
                RegistrationDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                IsActive = true,
                EmailConfirmed = true
            };

            await _userManager.SetEmailAsync(user, model.Email);

            await _userManager.SetUserNameAsync(user, uniqueUsername);

            user.CreatedByUserId = await GetCurentUserId();

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                _logger.LogInformation("New user created.");

                return View(model);
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        //Bellow you can find the methods used in that controller
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
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync();

            if (userProfile != null && Guid.TryParse(userProfile.Id.ToString(), out Guid userId))
            {
                return userId;
            }
            else
            { 
                return null;
            }
        }
    }
}





