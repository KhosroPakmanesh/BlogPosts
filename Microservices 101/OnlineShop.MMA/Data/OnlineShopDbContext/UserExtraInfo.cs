using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.MMA.Data.OnlineShopDbContext
{
    public class UserExtraInfo
    {
        public int IdUserExtraInfo { get; set; }
        public string FirstName { get; set; } = string.Empty;   
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

        public AspNetUser AspNetUser { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;

        public string FullName => FirstName + " " + LastName;
    }
}
