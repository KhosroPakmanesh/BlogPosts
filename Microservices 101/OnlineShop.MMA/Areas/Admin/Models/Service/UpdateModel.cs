using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Website.Presentation.Areas.Admin.ModelValidators;

namespace Website.Presentation.Areas.Admin.Models.Service
{
    public class UpdateModel
    {
        public UpdateModel()
        {
            CategorySelectListItems = new List<SelectListItem>();
            TagSelectListItems = new List<SelectListItem>();
            CategoryIds = new List<int>();
            TagIds = new List<int>();
        }

        //View and binding Models
        [Required]
        public int IdService { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; }


        [Required(ErrorMessage = "The describing ten-character summary should not be empty")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string TenCharacterSummary { get; set; }
        [Required(ErrorMessage = "The describing three-bullet points should not be empty")]
        public string ThreeBulletPoints { get;  set; }

        [Required]
        public string Body { get; set; }

        public List<SelectListItem> CategorySelectListItems { get; set; }
        [EnsureMinimumElementsAttribute(1, ErrorMessage = "At least a tag is required")]
        public List<int> CategoryIds { get; set; }

        public List<SelectListItem> TagSelectListItems { get; set; }
        [EnsureMinimumElementsAttribute(1, ErrorMessage = "At least a category is required")]
        public List<int> TagIds { get; set; }

        [Required]
        public bool IsPublishable { get; set; }
        [Required]
        public int ImportanceOrder { get; set; }
        public IFormFile Image { set; get; }
        public string ImageName { get; set; }
    }
}
