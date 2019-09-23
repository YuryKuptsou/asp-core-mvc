using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspCoreMvc.Infrastructure;
using aspCoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
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
                           select new Product
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
    }
}