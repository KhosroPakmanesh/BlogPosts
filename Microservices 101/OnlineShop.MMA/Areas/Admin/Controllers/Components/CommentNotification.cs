//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Threading.Tasks;
//using Website.Presentation.Areas.Admin.Models.Components;
//using Website.Presentation.Data;

//namespace Website.Presentation.Areas.Admin.Controllers.Components
//{
//    [ViewComponent]
//    public class CommentNotification : ViewComponent
//    {
//        private readonly BlogDbContext blogDbContext;

//        public CommentNotification(BlogDbContext blogDbContext)
//        {
//            this.blogDbContext = blogDbContext;
//        }
//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            var unreadComments = await blogDbContext.Comments
//                .Include(t => t.AspNetUser)
//                    .ThenInclude(t => t.UserExtraInfo)
//                .Where(t => !t.IsReviewed)
//                .ToListAsync();

//            var cultureInfo = new CultureInfo("en-us");

//            var nameMessagePairs = new List<NameMessagePair>();
//            foreach (var unreadComment in unreadComments)
//            {
//                nameMessagePairs.Add(new
//                NameMessagePair
//                {
//                    Name = unreadComment.CommenterId == null ?
//                        unreadComment.FullName : unreadComment.AspNetUser.UserExtraInfo.FullName,
//                    Message = unreadComment.Body.Length >= 30 ?
//                        unreadComment.Body.Substring(0, 30) : unreadComment.Body,
//                    CreateDateTime = unreadComment.CreationDateTime.ToString(cultureInfo),
//                    PostId = unreadComment.PostId
//                });
//            }

//            return View(new CommentNotificationModel
//            {
//                UnreadCommentCount = unreadComments.Count,
//                NameMessagePairs = nameMessagePairs
//            });
//        }
//    }
//}
