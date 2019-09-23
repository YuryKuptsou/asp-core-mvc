using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspCoreMvc.Infrastructure;
using aspCoreMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspCoreMvc.Controllers
{
    public class ProductController : Controller
    {
        private NorthwindContext _dbContext;

        public ProductController(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
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

            return View(products);
        }
    }
}