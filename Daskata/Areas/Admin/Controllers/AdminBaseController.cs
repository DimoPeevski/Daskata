using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using static Daskata.Core.Constants.RoleConstants;

namespace Daskata.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AdminRole)]
    public class AdminBaseController : Microsoft.AspNetCore.Mvc.Controller
    {
     
    }
}
