﻿using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class Order
{
    public int IdOrder { get; set; }

    public string BuyerId { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public byte Status { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();

    public Payment Payment { get; set; } = null!;
    public AspNetUser Buyer { get; set; } = null!;
}
