using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.Product
{
    public class UpdateModel
    {
        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int ProductTypeId { get; set; }
        [Required]
        public string ProductTypeName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value equal or bigger than {1}")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Description { get; set; } = string.Empty;

        public List<SelectListItem> ProductTypeSelectListItems { get; set; } = new List<SelectListItem>();
    }
}
