using Daskata.Core.Contracts.Network;
using Daskata.Core.ViewModels;
using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Daskata.Controllers
{
    [Authorize]
    public class NetworkController : Controller
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<NetworkController> _logger;
        private readonly INetworkService _networkService;

        public NetworkController(UserManager<UserProfile> userManager,
            ILogger<NetworkController> logger,
            INetworkService networkService)
        {
            _userManager = userManager;
            _logger = logger;
            _networkService = networkService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var loggedUser = await _userManager.GetUserAsync(User);
                var model = await _networkService.GetConnectedUsersAsync(loggedUser!);

                return View(model);
            }

            catch (InvalidOperationException ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            try
            {
                var loggedUser = await _userManager.GetUserAsync(User);
                if (loggedUser == null || !loggedUser.IsActive)
                {
                    return NotFound();
                }

                var model = await _networkService.GetUsersCreatedByAsync(loggedUser.Id);

                return View(model);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [Route("/Network/@{username}/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string username)
        {
            try
            {
                var model = await _networkService.GetUserForEditAsync(username);

                HttpContext.Session.SetString("CurrentUsername", model.Username);

                return View(model);
            }

            catch (InvalidOperationException ex)
            {
                return NotFound();
            }

            catch (ArgumentException ex)
            {
                return BadRequest();
            }
        }

        [Route("/Network/@{username}/Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUsername = HttpContext.Session.GetString("CurrentUsername");

            try
            {
                await _networkService.EditUserAsync(currentUsername, model);
                _logger.LogInformation($"User {currentUsername} was edited successfully.");
                return RedirectToAction("My", "Network");
            }

            catch (ArgumentException ex)
            {
                return BadRequest();
            }

            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Потребителят не е намерен.")
                {
                    return NotFound();
                }

                else
                {
                    return View(model);
                }
            }
        }

        [Route("/Network/@{username}/Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string username)
        {
            try
            {
                var model = await _networkService.GetUserProfileForDeletionAsync(username);

                return View(model);
            }

            catch (ArgumentException ex)
            {
                return BadRequest();
            }

            catch (InvalidOperationException ex)
            {
                return NotFound();
            }
        }

        [Route("/Network/@{username}/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(UserProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _networkService.DeactivateUserAsync(model.Username);
                _logger.LogInformation($"User with username {model.Username} was delete (marked as inactive).");

                return RedirectToAction("My", "Network");
            }

            catch (ArgumentException ex)
            {
                return BadRequest();
            }

            catch (InvalidOperationException ex)
            {
                return NotFound();
            }
        }
    }
}


