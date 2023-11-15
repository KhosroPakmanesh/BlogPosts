//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Threading.Tasks;
//using Website.Presentation.Areas.Admin.Models.Components;
//using Website.Presentation.Data;

//namespace Website.Presentation.Areas.Admin.Controllers.Components
//{
//    [ViewComponent]
//    public class ContactMessageNotification : ViewComponent
//    {
//        private readonly BlogDbContext blogDbContext;

//        public ContactMessageNotification(BlogDbContext blogDbContext)
//        {
//            this.blogDbContext = blogDbContext;
//        }
//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            var unreadContacts = await blogDbContext.Contacts
//                .Include(t => t.AspNetUser)
//                    .ThenInclude(t => t.UserExtraInfo)
//                .Where(t => !t.IsReviewed)
//                .ToListAsync();

//            var cultureInfo = new CultureInfo("en-us");

//            var nameMessagePairs = new List<NameMessagePair>();
//            foreach (var unreadContact in unreadContacts)
//            {
//                nameMessagePairs.Add(new
//                NameMessagePair
//                {
//                    Name = unreadContact.ContactorId == null ?
//                        unreadContact.FullName : unreadContact.AspNetUser.UserExtraInfo.FullName,
//                    Message = unreadContact.Body.Length >= 30 ?
//                        unreadContact.Body.Substring(0, 30) : unreadContact.Body,
//                    CreateDateTime = unreadContact.CreationDateTime.ToString(cultureInfo),
//                    IdContact=unreadContact.IdContact
//                });
//            }

//            return View(new ContactMessageNotificationModel
//            {
//                UnreadContactMessageCount = unreadContacts.Count,
//                NameMessagePairs = nameMessagePairs
//            });
//        }
//    }
//}
