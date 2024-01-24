using System;
using System.Collections.Generic;

namespace ConsoleApplication.Entities;

public partial class Order
{
    public int IdOrder { get; set; }
    public DateTime DateTime { get; set; }
    public int Status { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public Payment Payment { get; set; } = null!;
}
