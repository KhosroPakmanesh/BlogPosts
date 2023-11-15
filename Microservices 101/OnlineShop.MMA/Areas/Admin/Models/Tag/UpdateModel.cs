using System.ComponentModel.DataAnnotations;

namespace Website.Presentation.Areas.Admin.Models.Tag
{
    public class UpdateModel
    {
        [Required]
        public int IdTag { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Description { get; set; }
    }
}
