using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspCoreMvc.Infrastructure;
using aspCoreMvc.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace aspCoreMvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var categories = _categoryService.GetAll().Select(s => s.CategoryName);

            return View(categories);
        }
    }
}