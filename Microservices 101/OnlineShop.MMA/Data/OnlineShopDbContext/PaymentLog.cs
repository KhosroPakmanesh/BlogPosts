using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class PaymentLog
{
    public int IdPayment { get; set; }

    public string BuyerId { get; set; } = null!;

    public string BandAccountNumber { get; set; } = null!;

    public DateTime PaymentDateTime { get; set; }

    public decimal PaymentValue { get; set; }

    public virtual AspNetUser Buyer { get; set; } = null!;
}
