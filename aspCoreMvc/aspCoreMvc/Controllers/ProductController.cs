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
            var model = new ProductViewModel
            {
                Categories = GetCategories(),
                Suppliers = GetSuppliers()
            };
            ViewBag.Caption = "Create product";

            return View("Update", model);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var model = MapProductVM(_dbContext.Products.Find(id));
            model.Suppliers = GetSuppliers();
            model.Categories = GetCategories();
            ViewBag.Caption = "Update product";

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = GetCategories();
                model.Suppliers = GetSuppliers();

                return View(model);
            }


            if (model.Id == 0)
            {
                _dbContext.Products.Add(MapProduct(model));
            }
            else
            {
                _dbContext.Products.Update(MapProduct(model));
            }
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
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

        private Product MapProduct(ProductViewModel productViewModel)
        {
            return new Product
            {
                Id = productViewModel.Id,
                ProductName = productViewModel.ProductName,
                QuantityPerUnit = productViewModel.QuantityPerUnit,
                UnitPrice = productViewModel.UnitPrice,
                UnitsInStock = productViewModel.UnitsInStock,
                UnitsOnOrder = productViewModel.UnitsOnOrder,
                ReorderLevel = productViewModel.ReorderLevel,
                Discontinued = productViewModel.Discontinued,
                CategoryId = productViewModel.CategoryId,
                SupplierId = productViewModel.SupplierId
            };
        }

        private ProductViewModel MapProductVM(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
            };
        }
    }
}