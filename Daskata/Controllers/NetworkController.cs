using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Daskata.Core.Shared.Methods;

namespace Daskata.Controllers
{
    [Authorize]
    public class NetworkController : Controller
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly DaskataDbContext _context;
        private readonly ILogger<LoginUserFormModel> _logger;

        public NetworkController(UserManager<UserProfile> userManager, DaskataDbContext context, ILogger<LoginUserFormModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null || !loggedUser.IsActive)
            {
                return NotFound();
            }

            var usersCreatedByMe = await _context.Users
                .Where(u => u.CreatedByUserId == loggedUser!.Id)
                .ToListAsync();

            var model = usersCreatedByMe.Select(u => new UserProfileModel
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

            return View(model);
        }

        [Route("/Network/@{username}/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string username)
        {
            var userToEdit = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (userToEdit == null || !userToEdit.IsActive)
            {
                return NotFound();
            }

            var model = new EditUserFormModel
            {
                Username = userToEdit.UserName!,
                Email = userToEdit.Email!,
                FirstName = userToEdit.FirstName,
                LastName = userToEdit.LastName,
                PhoneNumber = userToEdit.PhoneNumber,
                AdditionalInfo = userToEdit.AdditionalInfo,
                ProfilePictureUrl = userToEdit.ProfilePictureUrl,
                Location = userToEdit.Location,
                School = userToEdit.School
            };

            return View(model);
        }

        [Route("/Network/@{username}/Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userUnderEdit = await _context.UserProfiles.FirstOrDefaultAsync(e => e.UserName == model.Username);

            userUnderEdit!.FirstName = model.FirstName;
            userUnderEdit.LastName = model.LastName;
            userUnderEdit.ProfilePictureUrl = model.ProfilePictureUrl;

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

            userUnderEdit.School = model.School;
            userUnderEdit.Location = model.Location;
            userUnderEdit.AdditionalInfo = model.AdditionalInfo;

            if (userUnderEdit.UserName != model.Username)
            {
                if (await UsernameExistsAsync(model.Username))
                {
                    ModelState.AddModelError("UsernameExists", "Потребителското име вече съществува.");
                    return View(model);
                }
            }

            userUnderEdit.UserName = model.Username;

            if (userUnderEdit.Email != model.Email)
            {
                if (await EmailExistsAsync(model.Email) && model.Email != "no@email.xyz" && model.Email != string.Empty)
                {
                    ModelState.AddModelError("EmailExists", "E-mail адресът име вече съществува.");
                    return View(model);
                }
            }

            userUnderEdit.Email = model.Email.ToLower();

            if (userUnderEdit.PhoneNumber != model.PhoneNumber && model.PhoneNumber != string.Empty)
            {
                if (await PhoneNumberExistsAsync(model.PhoneNumber!))
                {
                    ModelState.AddModelError("PhoneNumberExists", "Телефонът вече съществува.");
                    return View(model);
                }
            }

            userUnderEdit.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(userUnderEdit);
            if (!result.Succeeded)
            {
                return View(model);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"User with id {userUnderEdit.Id} was edited.");

            return RedirectToAction("My", "Network");
        }

        [Route("/Network/@{username}/Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string username)
        {
            var userToDelete = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            if (userToDelete == null || !userToDelete.IsActive)
            {
                return NotFound();
            }

            var model = new UserProfileModel
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

            return View(model);
        }

        [Route("/Network/@{username}/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(UserProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userUnderDelete = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == model.Username);

            userUnderDelete!.FirstName = model.FirstName;
            userUnderDelete.LastName = model.LastName;
            userUnderDelete.ProfilePictureUrl = "/lib/profile-pictures/default-profile-image.png";
            userUnderDelete.AdditionalInfo = string.Empty;
            userUnderDelete.School = string.Empty;
            userUnderDelete.Location = string.Empty;
            userUnderDelete.PhoneNumber = string.Empty;

            userUnderDelete.UserName = model.Username;
            userUnderDelete.Email = "no@email.xyz";
            userUnderDelete.IsActive = false;

            var result = await _userManager.UpdateAsync(userUnderDelete);
            if (!result.Succeeded)
            {
                return View(model);
            }

            _logger.LogInformation($"User with id {userUnderDelete.Id} was delete (marked as inactive).");

            return RedirectToAction("My", "Network");
        }

        // Methods used in class: NetworkController

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

    
