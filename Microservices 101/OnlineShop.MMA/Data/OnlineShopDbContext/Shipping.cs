using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class Shipping
{
    public int IdShipping { get; set; }

    public int OrderId { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
