using Microsoft.AspNetCore.Mvc;
using OnlineShop.MMA.Areas.Admin.Models.Components;

namespace Website.Presentation.Areas.Admin.Controllers.Components
{
    [ViewComponent]
    public class SideBar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var currentController = (string) ViewContext.RouteData.Values["Controller"]!;

            var sidebarModel = new SidebarModel
            {
                IsOrderMenuSelected = 
                    currentController == "Order" ||
                    currentController == "OrderItem",
                IsCartMenuSelected = 
                    currentController == "Cart" ||
                    currentController == "CartItem",
                IsProductMenuSelected = currentController == "Product",
                IsProductTypeMenuSelected = currentController == "ProductType",
                IsInventoryMenuSelected = currentController == "Inventory",
                IsShippingMenuSelected = 
                    currentController == "Shipping" ||
                    currentController == "ShippingLeg",
                IsDiscountMenuSelected = 
                    currentController == "Discount" ||
                    currentController == "DiscountBuyer",
            };

            return View(sidebarModel);
        }
    }
}
