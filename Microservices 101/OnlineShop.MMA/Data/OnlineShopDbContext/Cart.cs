using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class Cart
{
    public int IdCart { get; set; }

    public string? DiscountVoucher { get; set; }

    public string BuyerId { get; set; } = null!;
    public virtual AspNetUser Buyer { get; set; } = null!;

    public int DiscountId { get; set; }
    public virtual Discount Discount { get; set; } = null!;


    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
