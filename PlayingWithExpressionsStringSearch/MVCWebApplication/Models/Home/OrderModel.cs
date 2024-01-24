using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWebApplication.Models.Home
{
    public class OrderModel
    {
        public int IdOrder { get; set; }
        public string UserUserName { get; set; } = string.Empty;
        public string OrderDateTime { get; set; } = string.Empty;
        public int OrderStatus { get; set; }

        public string BankAccountNumber { get; set; } = null!;
        public string PaymentDateTime { get; set; } = string.Empty;
        public decimal PaymentValue { get; set; }
    }
}
