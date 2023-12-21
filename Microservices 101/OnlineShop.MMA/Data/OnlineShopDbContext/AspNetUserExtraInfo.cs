using System;
using System.Collections.Generic;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class AspNetUserExtraInfo
{
    public int IdUserExtraInfo { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? MobileNumber { get; set; }

    public string? BankAccountNumber { get; set; }

    public string UserId { get; set; } = null!;
    public virtual AspNetUser User { get; set; } = null!;
}
