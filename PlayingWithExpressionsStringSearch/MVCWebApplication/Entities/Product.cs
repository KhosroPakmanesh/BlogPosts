using System;
using System.Collections.Generic;

namespace MVCWebApplication.Entities;

public partial class Product
{
    public int IdProduct { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

}
