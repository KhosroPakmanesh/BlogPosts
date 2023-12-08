namespace OnlineShop.MMA.Areas.Admin.Models.DiscountBuyer
{
    public class DiscountBuyerModel
    {
        public int DiscountId { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public string BuyerUserName { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
    }
}
