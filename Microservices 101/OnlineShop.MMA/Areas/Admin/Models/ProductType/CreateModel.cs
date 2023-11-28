using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.ProductType
{
    public class CreateModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Description { get; set; }
    }
}
