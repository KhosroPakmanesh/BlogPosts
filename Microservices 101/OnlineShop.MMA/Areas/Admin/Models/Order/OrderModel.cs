using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.MMA.Areas.Admin.Models.Order
{
    public class OrderModel
    {
        public int IdOrder { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;
        public DateTime OrderDateTime { get; set; }
        public byte OrderStatus { get; set; }
    }
}
