using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineShop.MMA.Areas.Admin.Models.Discount
{
    public class UpdateModel
    {
        public int IdDiscount { get; set; }
        public string Voucher { get; set; } = string.Empty;
        public byte ReductionPercentage { get; set; } = 0;

        public List<SelectListItem> BuyerSelectListItems { get; set; } = new List<SelectListItem>();
        public List<string> BuyerIds { get; set; } = new List<string>();
    }
}
