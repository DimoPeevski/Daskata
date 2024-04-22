using Daskata.Core.Contracts.Profile;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Daskata.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<LoginUserFormModel> _logger;
        private readonly IProfileService _profileService;

        public ProfileController 
            (UserManager<UserProfile> userManager,
            ILogger<LoginUserFormModel> logger, 
            IProfileService profileService)
        {
            _userManager = userManager;
            _logger = logger;
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = _userManager.GetUserId(User);
            Guid userId;

            if (!Guid.TryParse(userIdString, out userId))
            {
                return NotFound();
            }

            var model = await _profileService.GetUserProfileModelAsync(userId);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [Route("/Profile/Preview/@{username}")]
        [HttpGet]
        public async Task<IActionResult> Preview (string username)
        {
            UserProfileModel model = await _profileService.GetUserPreviewModelAsync(username);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null || !loggedUser.IsActive)
            {
                return NotFound();
            }

            var model = await _profileService.GetEditUserFormModelAsync(loggedUser.Id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser == null || !loggedUser.IsActive)
            {
                return NotFound("Потребителят не е намерен.");
            }

            IdentityResult updateResult = await _profileService.UpdateUserAsync(loggedUser.Id, model);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            _logger.LogInformation($"User profile with ID {loggedUser.Id} updated successfully.");

            return RedirectToAction("Index", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null || !loggedUser.IsActive)
            {
                return NotFound();
            }

            var model = new ChangePasswordModel
            {
                ProfilePictureUrl = loggedUser!.ProfilePictureUrl,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdString = FetchUserId();
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return NotFound();
            }

            var result = await _profileService.ChangeUserPasswordAsync(userId, model.Password, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("Index", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> PersonalData()
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null || !loggedUser.IsActive)
            {
                return NotFound();
            }

            var model = new ChangePasswordModel
            {
                ProfilePictureUrl = loggedUser!.ProfilePictureUrl,
            };

            return View(model);
        }

        // Private methods used in class: ProfileController

        private string FetchUserId()
        {
            var userIdString = _userManager.GetUserId(User);
            Guid userId;

            if (Guid.TryParse(userIdString, out userId))
            {
                return userIdString;
            }
            else
            {
                throw new InvalidOperationException("User ID is not valid");
            }
        }
    }
}
