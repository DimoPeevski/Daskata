using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Daskata.Core.Constants.RoleConstants;

namespace Daskata.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AdminRole)]
    public class AdminBaseController : Controller
    {
     
    }
}
