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
        public string OrderDateTime { get; set; } = string.Empty;
        public byte OrderStatus { get; set; }

        public string BankAccountNumber { get; set; } = null!;
        public string PaymentDateTime { get; set; } = string.Empty;
        public decimal PaymentValue { get; set; }
    }
}
