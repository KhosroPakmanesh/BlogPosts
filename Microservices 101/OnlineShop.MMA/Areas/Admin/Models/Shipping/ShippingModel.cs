namespace OnlineShop.MMA.Areas.Admin.Models.Shipping
{
    public class ShippingModel
    {
        public int IdShipping { get; set; }
        public int OrderId { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;
        public bool IsShipped { get; set; } 
    }
}
