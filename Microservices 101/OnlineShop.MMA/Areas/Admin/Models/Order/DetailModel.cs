namespace OnlineShop.MMA.Areas.Admin.Models.Order
{
    public class DetailModel
    {
        public string BuyerUserName { get; set; } = string.Empty;
        public DateTime OrderDateTime { get; set; }
        public byte OrderStatus { get; set; }
    }
}
