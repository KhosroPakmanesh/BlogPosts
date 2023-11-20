namespace OnlineShop.MMA.Areas.Admin.Models.Discount
{
    public class DiscountModel
    {
        public int IdDiscount { get; set; }
        public string Voucher { get; set; } = string.Empty;
        public byte ReductionPercentage { get; set; } = 0;
    }
}
