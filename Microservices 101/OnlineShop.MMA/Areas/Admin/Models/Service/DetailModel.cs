using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Presentation.Areas.Admin.Models.Service
{
    public class DetailModel
    {
        public DetailModel()
        {
            CategorySelectListItems = new List<SelectListItem>();
            TagSelectListItems = new List<SelectListItem>();
            CategoryIds = new List<int>();
            TagIds = new List<int>();
        }

        //View and binding Models
        public int IdService { get; set; }
        public string Title { get; set; }
        public string TenCharacterSummary { get; internal set; }
        public string ThreeBulletPoints { get; internal set; }
        public string Body { get; set; }
        public string ImageName { get; set; }

        public List<SelectListItem> CategorySelectListItems { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<SelectListItem> TagSelectListItems { get; set; }
        public List<int> TagIds { get; set; }

        public bool IsPublishable { get; set; }
        public int ImportanceOrder { get; set; }

    }
}
