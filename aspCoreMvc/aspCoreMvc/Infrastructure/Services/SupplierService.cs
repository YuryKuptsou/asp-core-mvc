﻿using aspCoreMvc.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCoreMvc.Infrastructure.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly NorthwindContext _dbContext;

        public SupplierService(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _dbContext.Suppliers.AsNoTracking().ToList();
        }

    }
}
