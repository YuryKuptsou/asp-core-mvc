using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspCoreMvc.Infrastructure;
using aspCoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace aspCoreMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly NorthwindContext _dbContext;
        private readonly int _productCount;

        public ProductController(NorthwindContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            if (!int.TryParse(configuration["ProductCount"], out _productCount))
            {
                _productCount = 0;
            }
            
        }

        public IActionResult Index()
        {

            var products = from product in _dbContext.Products
                           join supplier in _dbContext.Suppliers on product.SupplierId equals supplier.Id
                           join category in _dbContext.Categories on product.CategoryId equals category.Id
                           select new ProductViewModel
                           {
                               Id = product.Id,
                               ProductName = product.ProductName,
                               QuantityPerUnit = product.QuantityPerUnit,
                               UnitPrice = product.UnitPrice,
                               UnitsInStock = product.UnitsInStock,
                               UnitsOnOrder = product.UnitsOnOrder,
                               ReorderLevel = product.ReorderLevel,
                               Discontinued = product.Discontinued,
                               CompanyName = supplier.CompanyName,
                               CategoryName = category.CategoryName
                           };
            if (_productCount == 0)
            {
                return View(products);
            }


            return View(products.Take(_productCount));
        }

        [HttpGet]
        public IActionResult Create()
        {
            var product = new ProductViewModel
            {
                Categories = GetCategories(),
                Suppliers = GetSuppliers()
            };

            return View("Update", product);
        }

        [HttpPost]
        public IActionResult Update(ProductViewModel model)
        {
            if (model.Id == 0)
            {
                _dbContext.Products.Add()
            }
            else
            {

            }
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var categories = _dbContext.Categories.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CategoryName }).ToList();
            categories.Insert(0, new SelectListItem { Value = "0", Text = "Select category" });

            return categories;
        }

        private IEnumerable<SelectListItem> GetSuppliers()
        {
            var suppliers = _dbContext.Suppliers.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CompanyName }).ToList();
            suppliers.Insert(0, new SelectListItem { Value = "0", Text = "Select supplier" });

            return suppliers;
        }

        private
    }
}