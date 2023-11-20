using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Website.Presentation.Areas.Admin.ModelValidators;

namespace OnlineShop.MMA.Areas.Admin.Models.Discount
{
    public class CreateModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Voucher { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public byte ReductionPercentage { get; set; } = 0;

        public List<SelectListItem> BuyerSelectListItems { get; set; } = new List<SelectListItem>();
        public List<string> BuyerIds { get; set; } = new List<string>();
    }
}
