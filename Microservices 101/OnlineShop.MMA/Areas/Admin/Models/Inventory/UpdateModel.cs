using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.Inventory
{
    public class UpdateModel
    {
        [Required]
        public int IdInventory { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "At least a product is required")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value equal or bigger than {1}")]
        public int Quantity { get; set; }

        public List<SelectListItem> ProductSelectListItems { get; set; } = new List<SelectListItem>();
    }
}
