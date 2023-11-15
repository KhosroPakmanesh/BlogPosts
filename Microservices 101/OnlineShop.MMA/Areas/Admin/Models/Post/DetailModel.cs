using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Presentation.Areas.Admin.Models.Post
{
    public class DetailModel
    {

        public DetailModel()
        {
            BranchComments = new Dictionary<Guid, List<CommentModel>>();
            CategorySelectListItems = new List<SelectListItem>();
            TagSelectListItems = new List<SelectListItem>();
            CategoryIds = new List<int>();
            TagIds = new List<int>();
        }

        //View and binding Models
        public int IdPost { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Introduction { get; internal set; }
        public string PublicationDateTime { get; set; }
        public string ModificationDateTime { get; set; }

        public List<SelectListItem> CategorySelectListItems { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<SelectListItem> TagSelectListItems { get; set; }
        public List<int> TagIds { get; set; }

        public bool IsPublishable { get; set; }
        public string ImageName { get; set; }

        public Dictionary<Guid, List<CommentModel>> BranchComments { get; set; }
        public int LastCommentId { get; set; }
    }
}
