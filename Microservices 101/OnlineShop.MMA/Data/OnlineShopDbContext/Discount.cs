using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class Discount
{
    public int IdDiscount { get; set; }

    public string Voucher { get; set; } = null!;

    public byte ReductionPercentage { get; set; }

    public virtual ICollection<BuyerDiscount> BuyerDiscounts { get; set; } = new List<BuyerDiscount>();
}
