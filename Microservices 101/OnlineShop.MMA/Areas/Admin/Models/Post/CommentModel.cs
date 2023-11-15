using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Presentation.Areas.Admin.Models.Post
{
    public class CommentModel
    {
        public int IdComment { get; set; }
        public Guid CommentBranchId { get; set; }
        public bool IsBranchHead { get; set; }
        public string FullName { get; set; }
        public string Body { get; set; }
        public DateTime CreationDateTime { get; set; }       
        public string AvatarName { get; set; }
        public bool IsPublishable { get; set; }        
        public bool IsReviewed { get; set; }
        public int PostId { get; set; }
    }
}
