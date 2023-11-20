using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class Order
{
    public int IdOrder { get; set; }

    public string BuyerId { get; set; } = null!;

    public DateTime OrderDateTime { get; set; }

    public byte OrderStatus { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderLog> OrderLogs { get; set; } = new List<OrderLog>();

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();

    public AspNetUser Buyer { get; set; } = null!;
}
