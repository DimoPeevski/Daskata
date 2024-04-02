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
    public class ProfileController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly ILogger<LoginFormModel> _logger;
        private readonly DaskataDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileController(SignInManager<UserProfile> signInManager,
                              UserManager<UserProfile> userManager,
                              DaskataDbContext context,
                              ILogger<LoginFormModel> logger,
                              IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            var model = new EditUserProfileInfoModel
            {
                Username = loggedUser.UserName,
                Email = loggedUser.Email,
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
            var model = new EditUserProfileInfoModel
            {
                Username = loggedUser.UserName,
                Email = loggedUser.Email,
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
        public async Task<IActionResult> Edit(EditUserProfileInfoModel model)
        {
            var loggedUser = await _userManager.GetUserAsync(User);

            return RedirectToAction("User", "Index");
        }

        public static string GetRegistrationMonthYear(string registrationDate)
        {
            DateTime date = DateTime.Parse(registrationDate);
            string rawMonth = date.ToString("MMMM", new System.Globalization.CultureInfo("bg-BG"));
            string month = char.ToUpper(rawMonth[0]) + rawMonth.Substring(1);
            string year = date.Year.ToString();

            return $"{month} {year}";
        }

        public static string TranslateRoleInBG(string roleName)
        {
            switch (roleName)
            {
                case Admin:
                    return "⭐⭐⭐⭐ Админ";
                case Manager:
                    return "⭐⭐⭐ Мениджър";
                case Teacher:
                    return "⭐⭐ Учител";
                case Student:
                    return "⭐ Ученик";
                default:
                    return roleName;
            }
        }

        public static bool IsUsernameMatch(string usernameFromModel, string usernameFromJS)
        {
            return usernameFromModel == usernameFromJS;
        }
    }
}