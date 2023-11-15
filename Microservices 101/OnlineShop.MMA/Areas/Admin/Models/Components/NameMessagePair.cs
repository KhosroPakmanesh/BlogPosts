using NuGet.Common;
using System;

namespace Website.Presentation.Areas.Admin.Models.Components
{
    public class NameMessagePair
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string CreateDateTime { get; set; }
        public int PostId { get; set; }
        public int IdContact { get;  set; }
    }
}
