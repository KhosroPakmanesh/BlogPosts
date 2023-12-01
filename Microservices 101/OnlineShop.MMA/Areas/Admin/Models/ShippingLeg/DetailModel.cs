using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.ShippingLeg
{
    public class DetailModel
    {
        public int ShippingId { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool IsShipped { get; set; }

        [Required]
        public string PreviousAction { get; set; } = string.Empty;
    }
}
