using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.ShippingLeg
{
    public class CreateModel
    {
        [Required]
        public int ShippingId { get; set; }

        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public bool IsShipped { get; set; }
    }
}
