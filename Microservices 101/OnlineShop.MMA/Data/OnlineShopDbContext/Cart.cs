using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class Cart
{
    public int IdCart { get; set; }

    public string BuyerId { get; set; } = null!;

    public string? DiscountVoucher { get; set; }

    public virtual AspNetUser Buyer { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
