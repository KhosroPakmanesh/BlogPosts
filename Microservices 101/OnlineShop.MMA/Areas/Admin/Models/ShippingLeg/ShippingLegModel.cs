namespace OnlineShop.MMA.Areas.Admin.Models.ShippingLeg
{
    public class ShippingLegModel
    {
        public int IdShippingLeg { get; set; }
        public int ShippingId { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool IsShipped { get; set; } 
    }
}
