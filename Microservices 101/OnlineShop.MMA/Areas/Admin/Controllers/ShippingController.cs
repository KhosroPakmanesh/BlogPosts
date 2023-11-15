//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Globalization;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;
//using System.Linq.Dynamic.Core;
//using Website.Presentation.Areas.Admin.Models.Comment;
//using Website.Presentation.Data;
//using Website.Presentation.Entities;

//namespace Website.Presentation.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Route("Admin/Post/{postId:int}/Comment/")]
//    [Authorize(Roles = "admin")]
//    public class CommentController : Controller
//    {
//        private readonly BlogDbContext _blogDbContext;

//        public CommentController(BlogDbContext blogDbContext)
//        {
//            _blogDbContext = blogDbContext;
//        }

//        [HttpGet]
//        [Route("~/Admin/Comment/Index")]
//        public async Task<IActionResult> Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        [Route("~/Admin/Comment/GetComments")]
//        public async Task<IActionResult> GetComments()
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

//                var expectedComments = _blogDbContext.Comments
//                    .Include(t => t.AspNetUser)
//                        .ThenInclude(t => t.UserExtraInfo)
//                    .AsQueryable();

//                if (!(string.IsNullOrEmpty(sortColumn)
//                    && string.IsNullOrEmpty(sortColumnDirection)))
//                {
//                    expectedComments = expectedComments.
//                        OrderBy(sortColumn + " " + sortColumnDirection);
//                }

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    expectedComments = expectedComments.Where(m =>
//                           m.FullName.Contains(searchValue)
//                        || m.Email.Contains(searchValue)
//                        || m.WebAddress.Contains(searchValue)
//                         // m.AspNetUser.UserExtraInfo.FullName.Contains(searchValue)
//                         //|| m.AspNetUser.Email.Contains(searchValue)
//                         //|| m.AspNetUser.UserExtraInfo.WebAddress.Contains(searchValue)
//                         || m.Body.Contains(searchValue));
//                }

//                var cultureInfo = new CultureInfo("en-us");
//                recordsTotal = await expectedComments.CountAsync();
//                var retrievedComments = await expectedComments.Skip(skip).Take(pageSize)
//                    .Select(t =>
//                    new
//                    {
//                        t.IdComment,
//                        t.PostId,
//                        Fullname = t.CommenterId == null ?
//                            t.FullName : t.AspNetUser.UserExtraInfo.FullName,
//                        Email = t.CommenterId == null ?
//                            t.Email : t.AspNetUser.Email,
//                        WebAddress = t.CommenterId == null ?
//                            t.WebAddress : t.AspNetUser.UserExtraInfo.WebAddress,
//                        Body = t.Body.Substring(0, 30),
//                        CreationDateTime = t.CreationDateTime
//                            .ToString("dd MMMM yyyy HH:mm:ss",cultureInfo),
//                        t.IsPublishable,
//                        t.IsReviewed
//                    }).
//                    ToListAsync();

//                var responseObject = new
//                {
//                    draw,
//                    recordsFiltered = recordsTotal,
//                    recordsTotal,
//                    data = retrievedComments
//                };

//                return Ok(responseObject);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }
//        [Route("Detail/{commentId:int}")]
//        [HttpGet]
//        public async Task<IActionResult> Detail([FromRoute] int postId, [FromRoute] int commentId)
//        {
//            var post = await _blogDbContext.Posts
//                .Include(p => p.Comments)
//                    .ThenInclude(t => t.AspNetUser)
//                        .ThenInclude(t => t.UserExtraInfo)
//                .FirstOrDefaultAsync(t => t.IdPost == postId);

//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            var comment = post.Comments.
//                FirstOrDefault(t => t.IdComment == commentId);
//            if (comment == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            var cultureInfo = new CultureInfo("en-us");
//            return View(new DetailModel
//            {
//                FullName = comment.CommenterId == null ?
//                        comment.FullName :
//                        comment.AspNetUser.UserExtraInfo.FullName,
//                Email = comment.CommenterId == null ?
//                        comment.Email :
//                        comment.AspNetUser.Email,
//                WebAddress = comment.CommenterId == null ?
//                        comment.WebAddress :
//                        comment.AspNetUser.UserExtraInfo.WebAddress,
//                Body = comment.Body,
//                CreationDateTime = comment.CreationDateTime.ToString(cultureInfo),
//                PostId = comment.PostId,
//                IsReviewed = comment.IsReviewed,
//                IsPublishable=comment.IsPublishable
//            });
//        }
//        [Route("Create/{commentBranchId:guid}")]
//        [HttpGet]
//        public async Task<IActionResult> Create([FromRoute] int postId, [FromRoute] Guid? commentBranchId)
//        {
//            var post = await _blogDbContext.Posts
//                .Include(p => p.Comments)
//                .FirstOrDefaultAsync(t => t.IdPost == postId);

//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            if (commentBranchId != null)
//            {
//                var comment = post.Comments.
//                    FirstOrDefault(t => t.CommentBranchId == commentBranchId);
//                if (comment == null)
//                {
//                    return RedirectToAction("Index", "Post");
//                }
//            }

//            return View(new CreateModel
//            {
//                CommentBranchId = commentBranchId,
//                PostId = postId
//            });
//        }
//        [Route("Create/{commentBranchId:guid}")]
//        [HttpPost]
//        public async Task<IActionResult> Create(CreateModel createModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(createModel);
//            }

//            var CommentBranchId = Guid.NewGuid();
//            if (createModel.CommentBranchId != null)
//            {
//                CommentBranchId = createModel.
//                    CommentBranchId.GetValueOrDefault();
//            }

//            var authenticatedUser = await _blogDbContext.AspNetUsers
//               .Include(t => t.UserExtraInfo)
//               .FirstOrDefaultAsync(t => t.Id ==
//                   User.FindFirstValue(ClaimTypes.NameIdentifier));
//            if (authenticatedUser != null)
//            {
//                _blogDbContext.Comments.Add(new Comment
//                {
//                    CommentBranchId = CommentBranchId,
//                    Body = createModel.Body,
//                    CreationDateTime = DateTime.Now,
//                    PostId = createModel.PostId,
//                    CommenterId = authenticatedUser.Id,
//                    IsPublishable = createModel.IsPublishable,
//                    IsReviewed = createModel.IsReviewed
//                });
//            }

//            await _blogDbContext.SaveChangesAsync();

//            return RedirectToAction("Update", "Post", 
//                new { id = createModel.PostId });
//        }
//        [Route("Update/{commentId:int}")]
//        [HttpGet]
//        public async Task<IActionResult> Update([FromRoute] int postId, [FromRoute] int commentId)
//        {
//            return await LoadUpdateModel(postId, commentId);
//        }
//        [Route("Update/{commentId:int}")]
//        [HttpPost]
//        public async Task<IActionResult> Update(UpdateModel updateModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return await LoadUpdateModel
//                    (updateModel.PostId, updateModel.IdComment,updateModel);
//            }

//            var post =await _blogDbContext.Posts
//                .Include(p => p.Comments)
//                .FirstOrDefaultAsync(t => t.IdPost == updateModel.PostId);

//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post"); ;
//            }

//            var comment = post.Comments.
//                FirstOrDefault(t => t.IdComment == updateModel.IdComment);
//            if (comment == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            comment.Body = updateModel.Body;
//            comment.IsReviewed = updateModel.IsReviewed;
//            comment.IsPublishable = updateModel.IsPublishable;            
//            _blogDbContext.Comments.Update(comment);

//            await _blogDbContext.SaveChangesAsync();

//            return RedirectToAction("Update", "Post", 
//                new { id = updateModel.PostId });
//        }
//        [Route("Delete/{commentId:int}")]
//        [HttpGet]
//        public async Task<IActionResult> Delete([FromRoute] int postId, [FromRoute] int commentId)
//        {
//            var post = await _blogDbContext.Posts
//                .Include(p => p.Comments)
//                .FirstOrDefaultAsync(t => t.IdPost == postId);

//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            var comment = post.Comments.
//                FirstOrDefault(t => t.IdComment == commentId);
//            if (comment == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            var branchCommentList = post.Comments.
//                Where(t => t.CommentBranchId == comment.CommentBranchId);

//            var branchHeadComment = branchCommentList
//                .OrderBy(t => t.CreationDateTime).FirstOrDefault();
//            if (comment.IdComment == branchHeadComment.IdComment)
//            {
//                _blogDbContext.Comments.RemoveRange(branchCommentList);
//            }
//            else
//            {
//                _blogDbContext.Comments.Remove(comment);
//            }

//            await _blogDbContext.SaveChangesAsync();

//            return RedirectToAction("Update", "Post",
//                new { id = postId });
//        }

//        public async Task<IActionResult> LoadUpdateModel
//            (int postId, int commentId, UpdateModel previousUpdateModel=null)
//        {
//            var post = await _blogDbContext.Posts
//                .Include(p => p.Comments)
//                .FirstOrDefaultAsync(t => t.IdPost == postId);

//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            var comment = post.Comments.
//                FirstOrDefault(t => t.IdComment == commentId);
//            if (comment == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            if (previousUpdateModel!= null)
//            {
//                return View(new UpdateModel
//                {
//                    IdComment = commentId,
//                    Body = previousUpdateModel.Body,
//                    PostId = previousUpdateModel.PostId,
//                    IsReviewed = previousUpdateModel.IsReviewed,
//                    IsPublishable =previousUpdateModel.IsPublishable
//                });
//            }

//            return View(new UpdateModel
//            {
//                IdComment = commentId,
//                Body = comment.Body,
//                PostId = comment.PostId,
//                IsReviewed = comment.IsReviewed,
//                IsPublishable = comment.IsPublishable
//            });
//        }
//    }
//}