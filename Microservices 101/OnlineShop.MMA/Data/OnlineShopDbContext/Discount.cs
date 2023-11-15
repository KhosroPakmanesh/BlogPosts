using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class Discount
{
    public int IdDiscount { get; set; }

    public string BuyerId { get; set; } = string.Empty;
    public string Voucher { get; set; } = string.Empty;
    public byte ReductionPercentage { get; set; } = 0;
    public bool IsUsed { get; set; } =false;

    public virtual AspNetUser Buyer { get; set; } = null!;
}
