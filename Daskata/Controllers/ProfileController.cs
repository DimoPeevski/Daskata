using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Daskata.Infrastructure.Shared.Constants;

namespace Daskata.Controllers
{
    public class ProfileController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<LoginUserFormModel> _logger;
        private readonly DaskataDbContext _context;

        public ProfileController(SignInManager<UserProfile> signInManager,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context,
                              ILogger<LoginUserFormModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            var model = new UserProfileModel
            {
                Username = loggedUser!.UserName!,
                Email = loggedUser.Email!,
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                PhoneNumber = loggedUser.PhoneNumber,
                AdditionalInfo = loggedUser.AdditionalInfo,
                ProfilePictureUrl = loggedUser.ProfilePictureUrl
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            var model = new EditUserFormModel
            {
                Username = loggedUser!.UserName!,
                Email = loggedUser.Email!,
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                PhoneNumber = loggedUser.PhoneNumber,
                AdditionalInfo = loggedUser.AdditionalInfo,
                ProfilePictureUrl = loggedUser.ProfilePictureUrl,
                Location = loggedUser.Location,
                School = loggedUser.School
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserFormModel model)
        {
            ModelState.Remove(nameof(model.ProfilePictureUrl));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser == null)
            {
                return NotFound();
            }

            loggedUser.FirstName = model.FirstName;
            loggedUser.LastName = model.LastName;

            if (model.School == null)
            {
                model.School = string.Empty;
            }
            if (model.Location == null)
            {
                model.Location = string.Empty;
            }
            if (model.AdditionalInfo == null)
            {
                model.AdditionalInfo = string.Empty;
            }
            if (model.PhoneNumber == null)
            {
                model.PhoneNumber = string.Empty;
            }

            loggedUser.School = model.School;
            loggedUser.Location = model.Location;
            loggedUser.AdditionalInfo = model.AdditionalInfo;

            if (loggedUser.UserName != model.Username)
            {
                if (await UsernameExistsAsync(model.Username))
                {
                    ModelState.AddModelError("Username", "Потребителското име вече съществува.");
                    return View(model);
                }
            }

            loggedUser.UserName = model.Username;

            if (loggedUser.Email != model.Email)
            {
                if (await EmailExistsAsync(model.Email)
                    && model.Email != "no@email.xyz"
                    && model.Email != string.Empty)
                {
                    ModelState.AddModelError("Email", "E-mail адресът име вече съществува.");
                    return View(model);
                }
            }

            loggedUser.Email = model.Email.ToLower();

            if (loggedUser.PhoneNumber != model.PhoneNumber
                && model.PhoneNumber != string.Empty)
            {
                if (await PhoneNumberExistsAsync(model.PhoneNumber!))
                {
                    ModelState.AddModelError("PhoneNumber", "Телефонът вече съществува.");
                    return View(model);
                }
            }

            loggedUser.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(loggedUser);
            if (!result.Succeeded)
            {
                return View(model);
            }

            _logger.LogInformation($"User with id {loggedUser.Id} was edited.");

            return RedirectToAction("Index", "Profile");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            var model = new ChangePasswordModel
            {
                ProfilePictureUrl = loggedUser!.ProfilePictureUrl,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", "Profile");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> PersonalData()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            var model = new ChangePasswordModel
            {
                ProfilePictureUrl = loggedUser!.ProfilePictureUrl,
            };

            return View(model);
        }

        private async Task<bool> UsernameExistsAsync(string username)
        {
            var context = _context.Users;
            return await context.AnyAsync(u => u.UserName == username);
        }

        private async Task<bool> EmailExistsAsync(string email)
        {
            var context = _context.Users;
            if (email.ToLower() == "no@email.xyz")
            {
                return false;
            }

            return await context.AnyAsync(u => u.Email == email);
        }

        private async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            var context = _context.Users;
            return await context.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
