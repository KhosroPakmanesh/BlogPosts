using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Presentation.Areas.Admin.Models.Comment
{
    public class DetailModel
    {
        public Guid CommentBranchId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string Body { get; set; }
        public string CreationDateTime { get; set; }
        public int PostId { get; set; }
        public bool IsReviewed { get;  set; }
        public bool IsPublishable { get;  set; }
    }
}
