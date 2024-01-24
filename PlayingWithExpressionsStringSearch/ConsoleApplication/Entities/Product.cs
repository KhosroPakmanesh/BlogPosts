using System;
using System.Collections.Generic;

namespace ConsoleApplication.Entities;

public partial class Product
{
    public int IdProduct { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

}
