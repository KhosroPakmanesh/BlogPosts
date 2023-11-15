using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Presentation.Areas.Admin.Models.Service
{
    public class ServiceModel
    {
        public ServiceModel()
        {
            Categories = new List<string>();
            Tags = new List<string>();
        }

        public int IdService { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual List<string> Categories { get; set; }
        public virtual List<string> Tags { get; set; }
        public bool IsPublishable { get; set; }
        public int ImportanceOrder { get; set; }
    }
}
