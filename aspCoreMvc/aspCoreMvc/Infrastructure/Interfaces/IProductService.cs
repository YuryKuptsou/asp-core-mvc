using aspCoreMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCoreMvc.Infrastructure.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductViewModel> GetAll(int count);
        Product Get(int id);

        int Create(Product product);
        void Update(Product product);

    }
}
