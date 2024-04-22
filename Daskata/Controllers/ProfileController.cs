using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Daskata.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<LoginUserFormModel> _logger;
        private readonly DaskataDbContext _context;

        public ProfileController(SignInManager<UserProfile> signInManager,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context,
            ILogger<LoginUserFormModel> logger, 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

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
                ProfilePictureUrl = loggedUser.ProfilePictureUrl,
                School = loggedUser.School,
                RegistrationDate = loggedUser.RegistrationDate.ToString(),
                Location = loggedUser.Location,

                Exams = await _context.Exams
                .Where(e => e.CreatedByUserId == loggedUser.Id)
                .Select(e => new FullExamViewModel
                {
                    Title = e.Title,
                    Description = e.Description,
                    Duration = (int)e.Duration.TotalMinutes,
                    TotalPoints = e.TotalPoints,
                    IsPublished = e.IsPublished,
                    CreationDate = e.CreationDate,
                    ExamUrl = e.ExamUrl,
                    CreatedByUserId = e.CreatedByUserId,
                    IsPublic = e.IsPublic,
                }).ToListAsync()
            };

            return View(model);
        }

        [Route("/Profile/Preview/@{username}")]
        [HttpGet]
        public async Task<IActionResult> Preview (string username)
        {
            var currentUser = await _userManager.FindByNameAsync(username);

            if (currentUser == null || !currentUser.IsActive)
            {
                
                return NotFound();
            }

            var model = new UserProfileModel
            {
                Username = currentUser!.UserName!,
                Email = currentUser.Email!,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhoneNumber = currentUser.PhoneNumber,
                AdditionalInfo = currentUser.AdditionalInfo,
                ProfilePictureUrl = currentUser.ProfilePictureUrl,
                School = currentUser.School,
                RegistrationDate = currentUser.RegistrationDate.ToString(),
                Location = currentUser.Location,

                Exams = await _context.Exams
                .Where(e => e.CreatedByUserId == currentUser.Id)
                .Select(e => new FullExamViewModel
                {
                    Title = e.Title,
                    Description = e.Description,
                    Duration = (int)e.Duration.TotalMinutes,
                    TotalPoints = e.TotalPoints,
                    IsPublished = e.IsPublished,
                    CreationDate = e.CreationDate,
                    ExamUrl = e.ExamUrl,
                    CreatedByUserId = e.CreatedByUserId
                }).ToListAsync()
            };

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

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserFormModel model)
        {
            ModelState.Remove(nameof(model.ProfilePictureUrl));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser == null || !loggedUser.IsActive)
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
                    ModelState.AddModelError("UsernameExists", "Потребителското име вече съществува.");
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
                    ModelState.AddModelError("EmailExists", "E-mail адресът име вече съществува.");
                    return View(model);
                }
            }

            loggedUser.Email = model.Email.ToLower();

            if (loggedUser.PhoneNumber != model.PhoneNumber
                && model.PhoneNumber != string.Empty)
            {
                if (await PhoneNumberExistsAsync(model.PhoneNumber!))
                {
                    ModelState.AddModelError("PhoneNumberExists", "Телефонът вече съществува.");
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
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null || !user.IsActive)
            {
                return NotFound();
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


        // Methods used in class: ProfileController

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
