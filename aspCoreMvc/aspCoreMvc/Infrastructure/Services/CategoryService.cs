using aspCoreMvc.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCoreMvc.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly NorthwindContext _dbContext;

        public CategoryService(NorthwindContext dbContext)
        {
            _dbContext = dbContext;    
        }

        public Category Get(int id)
        {
            return _dbContext.Categories.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _dbContext.Categories.AsNoTracking().ToList();
        }

        public byte[] GetImage(int id)
        {
            return _dbContext.Categories.Find(id)?.Picture;
        }

        public void Update(Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
        }
    }
}
