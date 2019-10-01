using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspCoreMvc.Infrastructure;
using aspCoreMvc.Infrastructure.Interfaces;
using aspCoreMvc.Infrastructure.Options;
using aspCoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace aspCoreMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly ICategoryService _categoryService;
        private readonly ProductOptions _options;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ISupplierService supplierService, ICategoryService categoryService,
            IOptionsSnapshot<ProductOptions> options, ILogger<ProductController> logger)
        {
            _productService = productService;
            _supplierService = supplierService;
            _categoryService = categoryService;
            _options = options.Value;
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Read max product count: {count}", _options.ProductCount);
            var products = _productService.GetAll(_options.ProductCount);

            return View(products);
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
            var model = MapProductVM(_productService.Get(id));
            model.Suppliers = GetSuppliers();
            model.Categories = GetCategories();
            ViewBag.Caption = "Update product";

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = GetCategories();
                model.Suppliers = GetSuppliers();

                return View(model);
            }

            if (model.Id == 0)
            {
                _productService.Create(MapProduct(model));
            }
            else
            {
                _productService.Update(MapProduct(model));
            }

            return RedirectToAction("Index");
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var categories =  _categoryService.GetAll().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CategoryName }).ToList();
            categories.Insert(0, new SelectListItem { Value = "0", Text = "Select category" });

            return categories;
        }

        private IEnumerable<SelectListItem> GetSuppliers()
        {
            var suppliers = _supplierService.GetAll().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CompanyName }).ToList();
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