using System;
using System.Collections.Generic;

namespace MVCWebApplication.Entities;

public partial class Payment
{
    public int IdPayment { get; set; }
    public string BankAccountNumber { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public decimal Value { get; set; }

    public int OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;
}
