using System.ComponentModel.DataAnnotations;
    
namespace OnlineShop.MMA.Areas.Admin.Models.Order 
{
    public class UpdateModel
    {
        [Required]
        public int IdOrder { get; set; }
        [Required]
        public string BuyerUserName { get; set; } = string.Empty;
        [Required]
        public DateTime OrderDateTime { get; set; }

        [Required]
        public byte OrderStatus { get; set; }
    }
}
