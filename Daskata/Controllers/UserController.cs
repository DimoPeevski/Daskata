using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly IUserStore<UserProfile> _userStore;
        private readonly DaskataDbContext _context;

        public UserController(SignInManager<UserProfile> signInManager, 
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
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

            UserProfile user = new UserProfile()
            {
                //CreatedByUserId = new Guid("01234567-89ab-cdef-0123-456789abcdef"),
            };

            await _userManager.SetEmailAsync(user, model.Email);
         
            await _userManager.SetUserNameAsync(user, uniqueUsername);

            var result = await _userManager.CreateAsync(user, model.Password);

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

            while (usernameExists)
            {
                Random rand = new Random();
                string randomNumbers = rand.Next(100000, 999999).ToString();
                username = $"user{randomNumbers}";

                usernameExists = await _context.UserProfiles.AnyAsync(u => u.UserName == username);
            }

            return username;
        }
    }
}





