using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class Discount
{
    public int IdDiscount { get; set; }

    public string Voucher { get; set; } = null!;

    public byte ReductionPercentage { get; set; }

    public virtual ICollection<DiscountBuyer> DiscountBuyers { get; set; } = new List<DiscountBuyer>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
