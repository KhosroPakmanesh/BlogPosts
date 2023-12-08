using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.DiscountBuyer
{
    public class UpdateModel
    {

        [Required]
        public int DiscountId { get; set; }

        [Required]
        public string BuyerUserName { get; set; } = string.Empty;

        [Required]
        public string BuyerId { get; set; } = string.Empty;

        [Required]
        public bool IsUsed { get; set; }
    }
}
