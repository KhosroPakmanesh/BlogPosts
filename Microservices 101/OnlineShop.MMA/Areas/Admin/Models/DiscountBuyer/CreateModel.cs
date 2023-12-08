using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.DiscountBuyer
{
    public class CreateModel
    {
        [Required]
        public int DiscountId { get; set; }

        [Required]
        public List<string> BuyerIds { get; set; } = new List<string>();
        public List<SelectListItem> BuyerSelectListItems { get; set; } = new List<SelectListItem>();


        [Required]
        public bool IsUsed { get; set; }
    }
}
