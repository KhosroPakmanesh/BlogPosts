using System.Collections.Generic;

namespace Website.Presentation.Areas.Admin.Models.Components
{
    public class CommentNotificationModel
    {
        public int UnreadCommentCount { get; set; }
        public List<NameMessagePair> NameMessagePairs { get; set; }
            = new List<NameMessagePair>();
    }
}
