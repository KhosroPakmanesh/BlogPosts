using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.MMA.Areas.Admin.Models.Cart
{
    public class CartModel
    {
        public int IdCart { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;
        public string DiscountVoucher { get; set; } = string.Empty;
    }
}
