using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.MMA.Areas.Admin.Models.PaymentHistory
{
    public class PaymentHistoryModel
    {
        public int IdPaymentHistory { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public DateTime PaymentDateTime { get; set; }
        public decimal PaymentValue { get; set; }
    }
}
