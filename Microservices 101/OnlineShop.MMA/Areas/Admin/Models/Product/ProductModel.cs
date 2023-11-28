using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.MMA.Areas.Admin.Models.Product
{
    public class ProductModel
    {
        public int IdProduct { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public string Name { get; set; }  = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
