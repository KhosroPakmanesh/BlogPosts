using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.Shipping
{
    public class UpdateModel
    {
        [Required]
        public int IdShipping { get; set; }

        public int OrderId { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;

        [Required]
        public bool IsShipped { get; set; }
    }
}
