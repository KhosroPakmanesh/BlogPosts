using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class PersonalInfo
{
    public int IdPersonalInfo { get; set; }

    public string UserId { get; set; } = null!;

    public string BankAccountNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
