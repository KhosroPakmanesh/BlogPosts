using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Presentation.Areas.Admin.Models.Post
{
    public class PostModel
    {
        public PostModel()
        {
            Categories = new List<string>();
            Tags = new List<string>();
        }

        public int IdPost { get; set; }
        public string Title { get; set; }
        public string ShortIntroduction { get; set; }
        public string PublicationDateTime { get; set; }
        public string ModificationDateTime { get; set; }
        public virtual List<string> Categories { get; set; }
        public virtual List<string> Tags { get; set; }
        public bool IsPublishable { get; set; }

    }
}
