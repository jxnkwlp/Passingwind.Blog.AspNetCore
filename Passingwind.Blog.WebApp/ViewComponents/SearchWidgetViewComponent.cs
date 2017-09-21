using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Passingwind.Blog.WebApp.ViewComponents
{
    public class SearchWidgetViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.FromResult(1);

            return View();
        }
    }
}
