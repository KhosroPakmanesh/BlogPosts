using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Presentation.Areas.Admin.Models.Components
{
    public class SidebarModel
    {
        public bool IsPostMenuSelected { get; set; }
        public bool IsCommentMenuSelected { get; set; }
        public bool IsCategoryMenuSelected { get; set; }
        public bool IsTagMenuSelected { get; set; }
        public bool IsContactsMenuSelected { get; set; }
        public bool IsCVMenuSelected { get; set; }
        public bool IsServiceMenuSelected { get; set; }
    }
}
