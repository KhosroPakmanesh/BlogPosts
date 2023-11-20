using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.MMA.Areas.Admin.Models.Shipping
{
    public class DetailModel
    {
        public int OrderId { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;

        public bool IsShipped { get; set; }
    }
}
