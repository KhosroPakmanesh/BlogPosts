using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class OrderHistory
{
    public int IdOrderHistory { get; set; }

    public int OrderId { get; set; }

    public byte OrderStatus { get; set; }

    public virtual Order Order { get; set; } = null!;
}
