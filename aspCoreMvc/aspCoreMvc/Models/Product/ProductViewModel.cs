using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace aspCoreMvc.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }


        [DisplayName("Supplier")]
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
