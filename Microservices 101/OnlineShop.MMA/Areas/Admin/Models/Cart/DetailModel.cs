namespace OnlineShop.MMA.Areas.Admin.Models.Cart
{
    public class DetailModel
    {
        public int IdCart { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;
        public string DiscountVoucher { get; set; } = string.Empty;
    }
}
