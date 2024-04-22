using Daskata.Core.Contracts.User;
using Daskata.Core.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Daskata.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<LoginUserFormModel> _logger;
        private readonly IUserService _userService;
   
        public UserController
            (IUserService userService,
            ILogger<LoginUserFormModel> logger)
        {
            _userService = userService;
            _logger = logger;
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

            var (result, user) = await _userService.LoginUserAsync(model.UserName, model.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Грешно потребителско име или парола.");
                return View(model);
            }

            _logger.LogInformation($"User {user?.UserName} logged in.");

            return RedirectToAction(nameof(Index), "Home");
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

            var createdByUserId = await _userService.GetCurentUserId();
            if (!createdByUserId.HasValue)
            {
                ModelState.AddModelError(string.Empty, "User ID could not be determined.");
                return View(model);
            }

            var result = await _userService.RegisterUserAsync(model, createdByUserId.Value);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            _logger.LogInformation($"New user was created successfully by user with ID {createdByUserId.Value}.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUserAsync();

            _logger.LogInformation("User logged out.");

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
