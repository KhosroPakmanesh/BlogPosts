using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class OrderLog
{
    public int IdOrderLog { get; set; }

    public int OrderId { get; set; }

    public byte Status { get; set; }

    public virtual Order Order { get; set; } = null!;
}
