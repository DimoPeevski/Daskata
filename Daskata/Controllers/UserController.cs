using Daskata.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Daskata.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly UserManager<UserProfile> _userManager;
        private readonly IUserStore<UserProfile> _userStore;

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
