using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Website.Presentation.Areas.Admin.ModelValidators;

namespace OnlineShop.MMA.Areas.Admin.Models.Discount
{
    public class CreateModel
    {
        public string Voucher { get; set; } = string.Empty;
        public byte ReductionPercentage { get; set; } = 0;

        public List<SelectListItem> BuyerSelectListItems { get; set; } = new List<SelectListItem>();
        public List<string> BuyerIds { get; set; } = new List<string>();
    }
}
