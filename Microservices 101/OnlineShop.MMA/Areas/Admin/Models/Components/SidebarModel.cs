using OnlineShop.MMA.Data.OnlineShopDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.MMA.Areas.Admin.Models.Components
{
    public class SidebarModel
    {
        public bool IsOrderMenuSelected { get; set; }
        public bool IsCartMenuSelected { get; set; }
        public bool IsProductMenuSelected { get; set; }
        public bool IsProductTypeMenuSelected { get; set; }
        public bool IsInventoryMenuSelected { get; set; }
        public bool IsShippingMenuSelected { get; set; }
        public bool IsDiscountMenuSelected { get; set; }
        public bool IsOrderHistoryMenuSelected { get; set; }
    }
}
