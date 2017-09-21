using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Passingwind.Blog.WebApp.Models;

namespace Passingwind.Blog.WebApp.ViewComponents
{
    public class CategoryListWidgetViewComponent : ViewComponent
    {
        private readonly CategoryManager _categoryManager;

        public CategoryListWidgetViewComponent(CategoryManager categoryManager)
        {
            this._categoryManager = categoryManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = _categoryManager.GetQueryable().ToList();

            var list = new List<CategoryViewModel>();

            foreach (var item in query)
            {
                var model = item.ToModel();
                model.Count = await _categoryManager.GetPostCountAsync(item.Id, false);

                list.Add(model);
            }

            return View(list);
        }

    }
}
