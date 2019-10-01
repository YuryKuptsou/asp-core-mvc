using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCoreMvc.Infrastructure.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAll();
    }
}
