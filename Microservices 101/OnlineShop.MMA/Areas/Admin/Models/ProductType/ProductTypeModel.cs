using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.MMA.Areas.Admin.Models.ProductType
{
    public class ProductTypeModel
    {
        public int IdProductType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
