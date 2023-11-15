//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Globalization;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Linq.Dynamic.Core;
//using Website.Presentation.Data;
//using Website.Presentation.Areas.Admin.Models.Contact;

//namespace Website.Presentation.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Roles = "admin")]
//    public class ContactController : Controller
//    {
//        private readonly BlogDbContext _blogDbContext;

//        public ContactController(BlogDbContext blogDbContext)
//        {
//            this._blogDbContext = blogDbContext;
//        }

//        [HttpGet]
//        public IActionResult Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> GetContacts()
//        {
//            try
//            {
//                var draw = Request.Form["draw"].FirstOrDefault();
//                var start = Request.Form["start"].FirstOrDefault();
//                var length = Request.Form["length"].FirstOrDefault();
//                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
//                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
//                var searchValue = Request.Form["search[value]"].FirstOrDefault();
//                int pageSize = length != null ? Convert.ToInt32(length) : 0;
//                int skip = start != null ? Convert.ToInt32(start) : 0;
//                int recordsTotal = 0;

//                var expectedContacts = _blogDbContext.Contacts.AsQueryable();

//                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
//                {
//                    expectedContacts = expectedContacts.OrderBy(sortColumn + " " + sortColumnDirection);
//                }

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    expectedContacts = expectedContacts.Where(
//                        m => m.Body.Contains(searchValue)
//                        || m.FullName.Contains(searchValue)
//                        || m.Email.Contains(searchValue)
//                        || m.WebAddress.Contains(searchValue));
//                }

//                var cultureInfo = new CultureInfo("en-us");
//                recordsTotal = await expectedContacts.CountAsync();
//                var retrievedContacts = await expectedContacts.Skip(skip).Take(pageSize).
//                    Select(t =>
//                    new
//                    {
//                        t.IdContact,
//                        FullName = t.ContactorId == null ?
//                            t.FullName : t.AspNetUser.UserExtraInfo.FullName,
//                        Email = t.ContactorId == null ?
//                            t.Email : t.AspNetUser.Email,
//                        WebAddress = t.ContactorId == null ?
//                            t.WebAddress : t.AspNetUser.UserExtraInfo.WebAddress,
//                        t.Body,
//                        CreationDateTime = t.CreationDateTime
//                            .ToString("dd MMMM yyyy HH:mm:ss",cultureInfo),
//                    }).ToListAsync();

//                var responseObject = new
//                {
//                    draw,
//                    recordsFiltered = recordsTotal,
//                    recordsTotal,
//                    data = retrievedContacts
//                };

//                return Ok(responseObject);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }
//        [HttpGet]
//        public async Task<IActionResult> Detail(int id)
//        {
//            var contact = await _blogDbContext.Contacts
//                .Include(t => t.AspNetUser)
//                    .ThenInclude(t => t.UserExtraInfo)
//                .FirstOrDefaultAsync(t => t.IdContact == id);

//            if (contact == null)
//            {
//                return RedirectToAction("Index", "Contact");
//            }

//            if (!contact.IsReviewed)
//            {
//                contact.IsReviewed = true;
//                await _blogDbContext.SaveChangesAsync();
//            }

//            var cultureInfo = new CultureInfo("en-us");
//            return View(new DetailModel
//            {
//                FullName = contact.ContactorId == null ?
//                                contact.FullName :
//                                contact.AspNetUser.UserExtraInfo.FullName,
//                Email = contact.ContactorId == null ?
//                                contact.Email :
//                                contact.AspNetUser.Email,
//                WebAddress = contact.ContactorId == null ?
//                                contact.WebAddress :
//                                contact.AspNetUser.UserExtraInfo.WebAddress,
//                Body = contact.Body,
//                CreationDateTime = contact.CreationDateTime.ToString(cultureInfo)
//            });
//        }
//        [HttpGet]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var contact = await _blogDbContext.Contacts
//                .FirstOrDefaultAsync(t => t.IdContact == id);

//            if (contact == null)
//            {
//                return RedirectToAction("Index", "Contact");
//            }

//            _blogDbContext.Contacts.Remove(contact);
//            await _blogDbContext.SaveChangesAsync(); ;

//            return RedirectToAction("Index", "Contact");
//        }
//    }
//}
