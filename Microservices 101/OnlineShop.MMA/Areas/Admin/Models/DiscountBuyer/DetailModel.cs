using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.DiscountBuyer
{
    public class DetailModel
    {
        public int DiscountId { get; set; }
        //public string BuyerId { get; set; } = string.Empty;
        public string BuyerUserName { get; set; } = string.Empty;
        public bool IsUsed { get; set; }

        [Required]
        public string PreviousAction { get; set; } = string.Empty;
    }
}
