﻿using Microsoft.AspNetCore.Mvc;

namespace Daskata.Components
{
    public class HeaderStudentMenuComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult<IViewComponentResult>(View("Student"));
        }
    }
}
