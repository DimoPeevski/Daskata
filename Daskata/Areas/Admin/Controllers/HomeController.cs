using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using static Daskata.Core.Constants.RoleConstants;

namespace Daskata.Areas.Admin.Controllers
{
    [Authorize(Roles = AdminRole)]
    public class HomeController : AdminBaseController
    {
        public IActionResult Dashboard()
        {
            return View();
        }
       
    }
}
