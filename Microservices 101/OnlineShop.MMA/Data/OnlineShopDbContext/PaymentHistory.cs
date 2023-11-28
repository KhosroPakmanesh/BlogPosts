using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class PaymentHistory
{
    public int IdPaymentHistory { get; set; }

    public string BuyerId { get; set; } = null!;

    public string BankAccountNumber { get; set; } = null!;

    public DateTime PaymentDateTime { get; set; }

    public decimal PaymentValue { get; set; }

    public virtual AspNetUser Buyer { get; set; } = null!;
}
