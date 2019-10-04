using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using aspCoreMvc.Infrastructure;
using aspCoreMvc.Infrastructure.Interfaces;
using aspCoreMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using aspCoreMvc.Infrastructure.Extensions;

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
            var categories = _categoryService.GetAll();

            return View(categories);
        }

        public IActionResult Image(int id)
        {
            var image = _categoryService.GetImage(id);
            if (image == null)
            {
                return NoContent();
            }

            return new FileStreamResult(new MemoryStream(image), "image/jpeg");
        }

        public IActionResult Update(int id)
        {
            var category = _categoryService.Get(id);
            if (category == null)
            {
                return NoContent();
            }
            var model = new CategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
            ViewBag.Caption = "Upload new image";

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Use jpeg, png, gif image formats");

                return View(model);
            }

            var category = _categoryService.Get(model.Id);
            using (var memoryStream = new MemoryStream())
            {
                model.Image.CopyTo(memoryStream);
                category.Picture = memoryStream.ToArray();
            }
            _categoryService.Update(category);

            return RedirectToAction("Index");
        }
    }


}