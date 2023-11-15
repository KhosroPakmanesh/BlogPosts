//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Globalization;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Identity;
//using System.Threading.Tasks;
//using System.Linq.Dynamic.Core;
//using Website.Presentation.Entities;
//using Website.Presentation.Areas.Admin.Models.Post;
//using Website.Presentation.Data;
//using Website.Presentation.Helpers;
//using Microsoft.AspNetCore.Http;
//using static System.Net.WebRequestMethods;
//using System.Drawing;
//using Libwebp.Standard;
//using Libwebp.Net.utility;
//using Libwebp.Net;

//namespace Website.Presentation.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Roles = "admin")]
//    public class PostController : Controller
//    {
//        private readonly IWebHostEnvironment webHostEnvironment;
//        private readonly BlogDbContext _blogDbContext;

//        public PostController(IWebHostEnvironment webHostEnvironment,
//            BlogDbContext blogDbContext)
//        {
//            this.webHostEnvironment = webHostEnvironment;
//            this._blogDbContext = blogDbContext;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> GetPosts()
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

//                var expectedPosts = _blogDbContext.Posts.AsQueryable();

//                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
//                {
//                    expectedPosts = expectedPosts.OrderBy(sortColumn + " " + sortColumnDirection);
//                }

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    expectedPosts = expectedPosts.Where(
//                        m => m.Title.Contains(searchValue)
//                        || m.Introduction.Contains(searchValue));
//                }

//                var cultureInfo = new CultureInfo("en-us");
//                recordsTotal = await expectedPosts.CountAsync();
//                var retrievedPosts = await expectedPosts
//                    .Skip(skip).Take(pageSize)
//                    .Select(t =>
//                    new
//                    {
//                        t.IdPost,
//                        t.Title,
//                        PublicationDateTime = t.PublicationDateTime.
//                            ToString("dd MMMM yyyy HH:mm:ss",cultureInfo),
//                        ModificationDateTime = t.ModificationDateTime == null ? 
//                            "Not Modified Yet" : t.ModificationDateTime.Value
//                            .ToString("dd MMMM yyyy HH:mm:ss", cultureInfo),
//                        t.IsPublishable
//                    })
//                    .ToListAsync();

//                var responseObject = new
//                {
//                    draw,
//                    recordsFiltered = recordsTotal,
//                    recordsTotal,
//                    data = retrievedPosts
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
//            var post = await _blogDbContext.Posts
//                .Include(p => p.Comments)
//                    .ThenInclude(t => t.AspNetUser)
//                        .ThenInclude(t => t.UserExtraInfo)
//                .Include(p => p.PostCategories)
//                .Include(p => p.PostTags)
//                .FirstOrDefaultAsync(p => p.IdPost == id);

//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            var categories = await _blogDbContext.Categories.ToListAsync();
//            var categorySelectListItems = new List<SelectListItem>();
//            foreach (var category in categories)
//            {
//                categorySelectListItems.Add(new SelectListItem
//                {
//                    Value = category.IdCategory.ToString(),
//                    Text = category.Title
//                });
//            }

//            var tags = await _blogDbContext.Tags.ToListAsync();
//            var tagSelectListItems = new List<SelectListItem>();
//            foreach (var tag in tags)
//            {
//                tagSelectListItems.Add(new SelectListItem
//                {
//                    Value = tag.IdTag.ToString(),
//                    Text = tag.Title
//                });
//            }

//            Dictionary<Guid, List<CommentModel>> branchCommentModels =
//                new Dictionary<Guid, List<CommentModel>>();
//            IEnumerable<IGrouping<Guid, Comment>> commentBranchs =
//                post.Comments.GroupBy(t => t.CommentBranchId);

//            var authenticatedUser = await _blogDbContext.AspNetUsers
//                .Include(t => t.UserExtraInfo)
//                .FirstOrDefaultAsync(t => t.Id ==
//                    User.FindFirstValue(ClaimTypes.NameIdentifier));

//            foreach (var commentBranch in commentBranchs)
//            {
//                var branchHeadComment = commentBranch.OrderBy
//                    (t => t.CreationDateTime).FirstOrDefault();

//                if (authenticatedUser != null)
//                {
//                    var commentModels = new List<CommentModel>();
//                    foreach (var comment in commentBranch.ToList())
//                    {
//                        commentModels.Add(
//                        new CommentModel
//                        {
//                            IdComment = comment.IdComment,
//                            IsBranchHead = comment.IdComment ==
//                                branchHeadComment.IdComment ? true : false,
//                            CommentBranchId = comment.CommentBranchId,
//                            FullName = comment.CommenterId == null ?
//                                comment.FullName :
//                                comment.AspNetUser.UserExtraInfo.FullName,
//                            Body = comment.Body,
//                            CreationDateTime = comment.CreationDateTime,
//                            AvatarName = comment.CommenterId == null ?
//                                string.Empty :
//                                comment.AspNetUser.UserExtraInfo.AvatarName,
//                            PostId = comment.PostId,
//                        });
//                    }
//                    branchCommentModels.Add
//                    (
//                        commentBranch.Key,
//                        commentModels
//                    );
//                }
//            }

//            int lastCommentId = default;
//            if (branchCommentModels.Any())
//            {
//                var lastBranchComment = branchCommentModels.LastOrDefault();
//                lastCommentId = lastBranchComment.Value.LastOrDefault().IdComment;
//            }

//            var cultureInfo = new CultureInfo("en-us");
//            return View(new DetailModel
//            {
//                IdPost = id,
//                Title = post.Title,
//                Body = post.Body,
//                Introduction = post.Introduction,
//                PublicationDateTime = post.PublicationDateTime.ToString(cultureInfo),
//                ModificationDateTime = post.ModificationDateTime != null ?
//                        post.ModificationDateTime.Value.ToString(cultureInfo) : string.Empty,
//                IsPublishable = post.IsPublishable,
//                BranchComments = branchCommentModels,
//                LastCommentId = lastCommentId,
//                CategoryIds = post.Categories.Select(t => t.IdCategory).ToList(),
//                CategorySelectListItems = categorySelectListItems,
//                TagIds = post.Tags.Select(t => t.IdTag).ToList(),
//                TagSelectListItems = tagSelectListItems,
//                ImageName = post.ImageName
//            });
//        }
//        [HttpGet]
//        public async Task<IActionResult> Create()
//        {
//            return await LoadCreateModel();
//        }
//        [HttpPost]
//        public async Task<IActionResult> Create(CreateModel createModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return await LoadCreateModel(createModel);
//            }

//            var postCategories = new List<PostCategory>();
//            foreach (var categoryId in createModel.CategoryIds)
//            {
//                postCategories.Add(new PostCategory
//                {
//                    CategoryId = categoryId
//                });
//            }

//            var postTags = new List<PostTag>();
//            foreach (var tagId in createModel.TagIds)
//            {
//                postTags.Add(new PostTag
//                {
//                    TagId = tagId
//                });
//            }

//            var uploadedImage = HttpContext.Request.Form.Files.FirstOrDefault();
//            var baseImageName = string.Empty;
//            if (uploadedImage != null)
//            {
//                baseImageName  = Guid.NewGuid().ToString() + ".webp";
//            }

//            _blogDbContext.Add(new Post
//            {
//                Title = createModel.Title,
//                Body = createModel.Body,
//                Introduction = createModel.Introduction,
//                PublicationDateTime = DateTime.Now,
//                ModificationDateTime = null,
//                IsPublishable = createModel.IsPublishable,
//                ImageName = baseImageName,
//                PostTags = postTags,
//                PostCategories = postCategories,
//                BloggerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
//            });

//            await _blogDbContext.SaveChangesAsync();

//            if (uploadedImage != null)
//            {
//                var uploadedPostsPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "posts");
                
//                var originalImagePath = Path.Combine(uploadedPostsPath, baseImageName);               
//                var originalBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 800, 450);
//                await ImageHelper.ConvertToWebP(originalImagePath, originalBitmap);

//                var thumbnailImagePath = Path.Combine(uploadedPostsPath, "sm-" + baseImageName);
//                var thumbnailBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 70, 39);
//                await ImageHelper.ConvertToWebP(thumbnailImagePath, thumbnailBitmap);
//            }

//            return RedirectToAction("Index", "Post");
//        }
//        [HttpGet]
//        public async Task<IActionResult> Update(int id)
//        {
//            return await LoadUpdateModel(id);
//        }
//        [HttpPost]
//        public async Task<IActionResult> Update(UpdateModel updateModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return await LoadUpdateModel(updateModel.IdPost, updateModel);
//            }

//            var post =await _blogDbContext.Posts
//                .Include(p => p.Comments)
//                .Include(p => p.PostCategories)
//                .Include(p => p.PostTags)
//                .FirstOrDefaultAsync(p => p.IdPost == updateModel.IdPost);
//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            var firstNotSecondCategoryIds = post.PostCategories.Select(pc => pc.CategoryId).ToList()
//                .Except(updateModel.CategoryIds).ToList();
//            var secondNotFirstCategoryIds = updateModel.CategoryIds
//                .Except(post.PostCategories.Select(pc => pc.CategoryId)).ToList();
//            if (firstNotSecondCategoryIds.Any() || secondNotFirstCategoryIds.Any())
//            {
//                var linkedPostCategories = _blogDbContext.PostCategories
//                    .Where(pc => pc.PostId == updateModel.IdPost);
//                _blogDbContext.RemoveRange(linkedPostCategories);
//                var postCategories = new List<PostCategory>();
//                foreach (var categoryId in updateModel.CategoryIds)
//                {
//                    postCategories.Add(new PostCategory
//                    {
//                        PostId = post.IdPost,
//                        CategoryId = categoryId
//                    });
//                }
//                _blogDbContext.AddRange(postCategories);
//            }

//            var firstNotSecondTagIds = post.PostTags.Select(pc => pc.TagId).ToList()
//                    .Except(updateModel.TagIds).ToList();
//            var secondNotFirstTagIds = updateModel.TagIds
//                    .Except(post.PostTags.Select(pc => pc.TagId)).ToList();
//            if (firstNotSecondTagIds.Any() || secondNotFirstTagIds.Any())
//            {
//                var linkedPostTags = _blogDbContext.PostTags
//                    .Where(pc => pc.PostId == updateModel.IdPost);
//                _blogDbContext.RemoveRange(linkedPostTags);
//                var postTags = new List<PostTag>();
//                foreach (var TagId in updateModel.TagIds)
//                {
//                    postTags.Add(new PostTag
//                    {
//                        PostId = post.IdPost,
//                        TagId = TagId
//                    });
//                }
//                _blogDbContext.AddRange(postTags);
//            }

//            string previousImageName = post.ImageName;
//            var uploadedImage = HttpContext.Request.Form.Files.FirstOrDefault();
//            string baseImageName = string.Empty;
//            if (uploadedImage != null)
//            {
//                post.ImageName = baseImageName = Guid.NewGuid().ToString() + ".webp";                
//            }

//            post.Title = updateModel.Title;
//            post.Body = updateModel.Body;
//            post.Introduction = updateModel.Introduction;
//            post.IsPublishable = updateModel.IsPublishable;
//            post.ModificationDateTime = DateTime.Now;

//            await _blogDbContext.SaveChangesAsync();

//            if (uploadedImage != null)
//            {
//                var uploadedPostsPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "posts");

//                var previousOriginalImagePath = 
//                    Path.Combine(uploadedPostsPath, previousImageName);
//                var file = new FileInfo(previousOriginalImagePath);
//                if (file.Exists)
//                {
//                    file.Delete();
//                }
//                var originalImagePath = Path.Combine(uploadedPostsPath, baseImageName);
//                var originalBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 800, 450);
//                await ImageHelper.ConvertToWebP(originalImagePath, originalBitmap);

//                var previousThumbnailImagePath =
//                    Path.Combine(uploadedPostsPath, "sm-" + previousImageName);
//                file = new FileInfo(previousThumbnailImagePath);
//                if (file.Exists)
//                {
//                    file.Delete();
//                }
//                var thumbnailImagePath = Path.Combine(uploadedPostsPath, "sm-" + baseImageName);
//                var thumbnailBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 70, 39);
//                await ImageHelper.ConvertToWebP(thumbnailImagePath, thumbnailBitmap);
//            }

//            return RedirectToAction("Index", "Post");
//        }
//        [HttpGet]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var post = await _blogDbContext.Posts
//               .Include(p => p.Comments)
//               .Include(p => p.PostCategories)
//               .Include(p => p.PostTags)
//               .FirstOrDefaultAsync(t => t.IdPost == id);

//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            _blogDbContext.Comments.RemoveRange(post.Comments);
//            _blogDbContext.PostCategories.RemoveRange(post.PostCategories);
//            _blogDbContext.PostTags.RemoveRange(post.PostTags);
//            _blogDbContext.Posts.Remove(post);

//            await _blogDbContext.SaveChangesAsync();

//            var uploadedPostsPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "posts");
//            var previousOriginalImagePath =
//                Path.Combine(uploadedPostsPath, post.ImageName);
//            var file = new FileInfo(previousOriginalImagePath);
//            if (file.Exists)
//            {
//                file.Delete();
//            }

//            var previousThumbnailImagePath =
//                Path.Combine(uploadedPostsPath, "sm-" + post.ImageName);
//            file = new FileInfo(previousThumbnailImagePath);
//            if (file.Exists)
//            {
//                file.Delete();
//            }

//            return RedirectToAction("Index", "Post");
//        }
//        public async Task<IActionResult> LoadUpdateModel
//            (int id, UpdateModel previousUpdateModel = null)
//        {
//            var post =await _blogDbContext.Posts
//                .Include(p => p.Comments)
//                    .ThenInclude(t => t.AspNetUser)
//                        .ThenInclude(t => t.UserExtraInfo)
//                .Include(p => p.PostCategories)
//                .Include(p => p.PostTags)
//                .FirstOrDefaultAsync(p => p.IdPost == id);

//            if (post == null)
//            {
//                return RedirectToAction("Index", "Post");
//            }

//            var categories = await _blogDbContext.Categories.ToListAsync();
//            var categorySelectListItems = new List<SelectListItem>();
//            foreach (var category in categories)
//            {
//                categorySelectListItems.Add(new SelectListItem
//                {
//                    Value = category.IdCategory.ToString(),
//                    Text = category.Title
//                });
//            }

//            var tags = await _blogDbContext.Tags.ToListAsync();
//            var tagSelectListItems = new List<SelectListItem>();
//            foreach (var tag in tags)
//            {
//                tagSelectListItems.Add(new SelectListItem
//                {
//                    Value = tag.IdTag.ToString(),
//                    Text = tag.Title
//                });
//            }

//            Dictionary<Guid, List<CommentModel>> branchCommentModels =
//                new Dictionary<Guid, List<CommentModel>>();
//            IEnumerable<IGrouping<Guid, Comment>> commentBranchs =
//                post.Comments.GroupBy(t => t.CommentBranchId);

//            foreach (var commentBranch in commentBranchs)
//            {
//                var branchHeadComment = commentBranch.OrderBy
//                    (t => t.CreationDateTime).FirstOrDefault();

//                var commentModels = new List<CommentModel>();
//                foreach (var comment in commentBranch.ToList())
//                {
//                    commentModels.Add(
//                        new CommentModel
//                        {
//                            IdComment = comment.IdComment,
//                            IsBranchHead = comment.IdComment == branchHeadComment.IdComment ? true : false,
//                            CommentBranchId = comment.CommentBranchId,
//                            FullName = comment.CommenterId == null ?
//                                comment.FullName :
//                                comment.AspNetUser.UserExtraInfo.FullName,
//                            Body = comment.Body,
//                            CreationDateTime = comment.CreationDateTime,
//                            AvatarName = comment.CommenterId == null ?
//                                string.Empty :
//                                comment.AspNetUser.UserExtraInfo.AvatarName,
//                            PostId = comment.PostId,
//                            IsPublishable=comment.IsPublishable,
//                            IsReviewed=comment.IsReviewed
//                        }
//                    );
//                }
//                branchCommentModels.Add
//                (
//                    commentBranch.Key,
//                    commentModels
//                );
//            }

//            int lastCommentId = default;
//            if (branchCommentModels.Any())
//            {
//                var lastBranchComment = branchCommentModels.LastOrDefault();
//                lastCommentId = lastBranchComment.Value.LastOrDefault().IdComment;
//            }

//            if (previousUpdateModel != null)
//            {
//                var updateModel = new UpdateModel
//                {
//                    IdPost = previousUpdateModel.IdPost,
//                    Title = previousUpdateModel?.Title,
//                    Body = previousUpdateModel?.Body,
//                    Introduction = previousUpdateModel.Introduction,
//                    CategoryIds = previousUpdateModel?.CategoryIds,
//                    TagIds = previousUpdateModel?.TagIds,
//                    Image = previousUpdateModel?.Image,
//                    IsPublishable = previousUpdateModel == null ?
//                        false : previousUpdateModel.IsPublishable,
//                    CategorySelectListItems = categorySelectListItems,
//                    TagSelectListItems = tagSelectListItems,
//                    ImageName = post.ImageName,
//                    BranchComments = branchCommentModels,
//                    LastCommentId = lastCommentId,
//                };

//                return View(updateModel);
//            }
//            return View(new UpdateModel
//            {
//                IdPost = id,
//                Title = post.Title,
//                Body = post.Body,
//                Introduction = post.Introduction,
//                CategoryIds = post.Categories.Select(t => t.IdCategory).ToList(),
//                TagIds = post.Tags.Select(t => t.IdTag).ToList(),
//                IsPublishable = post.IsPublishable,
//                CategorySelectListItems = categorySelectListItems,
//                TagSelectListItems = tagSelectListItems,
//                ImageName = post.ImageName ,
//                BranchComments = branchCommentModels,
//                LastCommentId = lastCommentId,
//            });
//        }
//        public async Task<IActionResult> LoadCreateModel
//            (CreateModel previousCreateModel = null)
//        {
//            var categories = await _blogDbContext.Categories.ToListAsync();
//            var categorySelectListItems = new List<SelectListItem>();
//            foreach (var category in categories)
//            {
//                categorySelectListItems.Add(new SelectListItem
//                {
//                    Value = category.IdCategory.ToString(),
//                    Text = category.Title
//                });
//            }

//            var tags = await _blogDbContext.Tags.ToListAsync();
//            var tagSelectListItems = new List<SelectListItem>();
//            foreach (var tag in tags)
//            {
//                tagSelectListItems.Add(new SelectListItem
//                {
//                    Value = tag.IdTag.ToString(),
//                    Text = tag.Title
//                });
//            }

//            var createModel = new CreateModel
//            {
//                Title = previousCreateModel?.Title,
//                Body = previousCreateModel?.Body,
//                Introduction = previousCreateModel?.Introduction,
//                CategoryIds = previousCreateModel?.CategoryIds,
//                TagIds = previousCreateModel?.TagIds,
//                Image = previousCreateModel?.Image,
//                IsPublishable = previousCreateModel == null ?
//                    false : previousCreateModel.IsPublishable,
//                CategorySelectListItems = categorySelectListItems,
//                TagSelectListItems = tagSelectListItems,
//            };

//            return View(createModel);
//        }
//    }
//}
