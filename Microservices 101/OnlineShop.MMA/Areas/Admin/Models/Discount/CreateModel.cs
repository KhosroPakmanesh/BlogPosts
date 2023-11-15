using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.Discount
{
    public class CreateModel
    {
        public string BuyerId { get; set; } = string.Empty;
        public string Voucher { get; set; } = string.Empty;
        public byte ReductionPercentage { get; set; } = 0;
        public bool IsUsed { get; set; } = false;
    }
}
