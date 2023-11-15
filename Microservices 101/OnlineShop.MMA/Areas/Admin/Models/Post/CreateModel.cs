using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Website.Presentation.Areas.Admin.ModelValidators;

namespace Website.Presentation.Areas.Admin.Models.Post
{
    public class CreateModel
    {
        public CreateModel()
        {
            CategorySelectListItems = new List<SelectListItem>();
            TagSelectListItems = new List<SelectListItem>();
        }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; }

        [Required(ErrorMessage = "The body should not be empty")]
        public string Body { get; set; }

        [Required]
        public string Introduction { get; set; }

        public List<SelectListItem> CategorySelectListItems { get; set; }
        [EnsureMinimumElements(1, ErrorMessage = "At least a category is required")]
        public List<int> CategoryIds { get; set; }

        public List<SelectListItem> TagSelectListItems { get; set; }
        [EnsureMinimumElementsAttribute(1, ErrorMessage = "At least a tag is required")]
        public List<int> TagIds { get; set; }

        [Required]
        public bool IsPublishable { get; set; }

        public IFormFile Image { set; get; }
    }
}
