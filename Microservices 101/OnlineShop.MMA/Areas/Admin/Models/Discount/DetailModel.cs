namespace OnlineShop.MMA.Areas.Admin.Models.Discount
{
    public class DetailModel
    {
        public int IdDiscount { get; set; } 
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Voucher { get; set; } = string.Empty;
        public byte ReductionPercentage { get; set; } = 0;
        public bool IsUsed { get; set; } = false;
    }
}
