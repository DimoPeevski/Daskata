using Microsoft.AspNetCore.Mvc;

namespace Daskata.Components
{
    public class HeaderMenuComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult<IViewComponentResult>(View());
        }
    }
}
