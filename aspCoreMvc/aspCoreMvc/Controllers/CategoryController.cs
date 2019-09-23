using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspCoreMvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace aspCoreMvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NorthwindContext _dbContext;

        public CategoryController(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var categories = _dbContext.Categories.Select(s => s.CategoryName).ToList();

            return View(categories);
        }
    }
}