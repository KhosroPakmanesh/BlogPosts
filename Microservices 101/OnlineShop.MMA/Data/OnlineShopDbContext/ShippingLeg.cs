using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class ShippingLeg
{
    public int IdShippingLeg { get; set; }

    public int ShippingId { get; set; }

    public string Address { get; set; } = null!;

    public bool IsShipped { get; set; }

    public virtual Shipping Shipping { get; set; } = null!;
}
