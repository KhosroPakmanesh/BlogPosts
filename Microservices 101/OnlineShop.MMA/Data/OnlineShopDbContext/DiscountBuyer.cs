using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class DiscountBuyer
{
    public string BuyerId { get; set; } = null!;

    public int DiscountId { get; set; }

    public bool IsUsed { get; set; }

    public virtual AspNetUser Buyer { get; set; } = null!;

    public virtual Discount Discount { get; set; } = null!;
}
