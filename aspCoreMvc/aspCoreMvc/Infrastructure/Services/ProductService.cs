using aspCoreMvc.Infrastructure.Interfaces;
using aspCoreMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCoreMvc.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _dbContext;

        public ProductService(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Product Get(int id)
        {
            return _dbContext.Products.Find(id);
        }

        public IEnumerable<ProductViewModel> GetAll(int count)
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
            if (count == 0)
            {
                return products;
            }
            else
            {
                return products.Take(count);
            }
        }

        public int Create(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return product.Id;
        }

        public void Update(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }
    }
}
