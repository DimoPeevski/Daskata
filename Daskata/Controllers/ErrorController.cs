using Daskata.Core.Services.Error;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Daskata.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErrorService _errorService;

        public ErrorController(IHttpContextAccessor httpContextAccessor, IErrorService errorService)
        {
            _httpContextAccessor = httpContextAccessor;
            _errorService = errorService;
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            var currentUserId = await GetCurentUserId();
            await _errorService.LogAccessDeniedAsync(currentUserId);
            return View();
        }

        [HttpGet]
        [Route("Error/404")]
        public ActionResult Error404NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        // Private methods used in class: ErrorController

        private async Task<Guid?> GetCurentUserId() 
        { 
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier); 
            
            if (userIdClaim != null && Guid.TryParse(userIdClaim, out Guid parsedUserId)) 
            { 
                return await Task.FromResult(parsedUserId); 
            } 
            else 
            { 
                return await Task.FromResult<Guid?>(null); 
            } 
        }
    }
}
  
