using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineShop.MMA.Areas.Admin.Models.Discount
{
    public class DetailModel
    {
        public int IdDiscount { get; set; } 

        public string Voucher { get; set; } = string.Empty;
        public byte ReductionPercentage { get; set; } = 0;
    }
}
