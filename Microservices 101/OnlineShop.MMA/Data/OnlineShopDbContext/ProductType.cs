using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class ProductType
{
    public int IdProductType { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
