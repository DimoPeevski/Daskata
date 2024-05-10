using Microsoft.AspNetCore.Mvc;

namespace Daskata.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Dashboard()
        {
            return View();
        }       
    }
}
