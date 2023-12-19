using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.Cart 
{
    public class UpdateModel
    {
        [Required]
        public int IdCart { get; set; }

        [Required]
        public string BuyerUserName { get; set; } = string.Empty;

        [Required]
        public int? DiscountId { get; set; }

        public List<SelectListItem> DiscountSelectListItems { get; set; } = new List<SelectListItem>();
    }
}
