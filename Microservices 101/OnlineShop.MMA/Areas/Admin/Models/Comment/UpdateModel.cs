using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Presentation.Areas.Admin.Models.Comment
{
    public class UpdateModel
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public int IdComment { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Body { get; set; }

        [Required]
        public bool IsPublishable { get; set; }

        [Required]
        public bool IsReviewed { get;  set; }
    }
}
