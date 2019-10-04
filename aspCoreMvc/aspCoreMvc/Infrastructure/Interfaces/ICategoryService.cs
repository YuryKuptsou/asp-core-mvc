using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCoreMvc.Infrastructure.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();

        Category Get(int id);

        byte[] GetImage(int id);

        void Update(Category category);

    }
}
