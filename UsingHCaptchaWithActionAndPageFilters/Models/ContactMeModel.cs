using System.ComponentModel.DataAnnotations;

namespace UsingHCaptchaWithActionAndPageFilters.Models
{
    public class ContactUsModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Email { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Body { get; set; }

        [Url]        
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string WebAddress { get; set; }
    }
}
