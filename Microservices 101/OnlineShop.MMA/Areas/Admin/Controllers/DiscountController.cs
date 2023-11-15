using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.Discount;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class DiscountController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public DiscountController(IWebHostEnvironment webHostEnvironment,
            OnlineShopDbContext onlineShopDbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _onlineShopDbContext = onlineShopDbContext;
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> GetDiscounts()
        //{
        //    try
        //    {
        //        var draw = Request.Form["draw"].FirstOrDefault();
        //        var start = Request.Form["start"].FirstOrDefault();
        //        var length = Request.Form["length"].FirstOrDefault();
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0;

        //        var expectedDiscounts = _onlineShopDbContext.Discounts.AsQueryable();

        //        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        //        {
        //            expectedDiscounts = expectedDiscounts.OrderBy(sortColumn + " " + sortColumnDirection);
        //        }

        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            expectedDiscounts = expectedDiscounts.Where(
        //                m => m.Voucher.Contains(searchValue) ||
        //                m.ReductionPercentage.ToString() == searchValue ||
        //                m.IsUsed.ToString() == searchValue);
        //        }

        //        recordsTotal = await expectedDiscounts.CountAsync();
        //        var retrievedPosts = await expectedDiscounts
        //            .Skip(skip).Take(pageSize)
        //            .Select(t =>
        //            new
        //            {
        //                t.IdDiscount,
        //                t.Buyer.UserExtraInfo.FullName,
        //                t.Voucher,
        //                t.ReductionPercentage,
        //                t.IsUsed,
        //            }).ToListAsync();

        //        var responseObject = new
        //        {
        //            draw,
        //            recordsFiltered = recordsTotal,
        //            recordsTotal,
        //            data = retrievedPosts
        //        };

        //        return Ok(responseObject);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> Detail(int id)
        //{
        //    var discount = await _onlineShopDbContext.Discounts
        //        .Include(p => p.Buyer)                    
        //            .ThenInclude(t => t.UserExtraInfo)
        //        .FirstOrDefaultAsync(p => p.IdDiscount == id);

        //    if (discount == null)
        //    {
        //        return RedirectToAction("Index", "Discount");
        //    }

        //    var users = await _onlineShopDbContext.AspNetUsers.ToListAsync();
        //    var userSelectListItems = new List<SelectListItem>();
        //    foreach (var user in users)
        //    {
        //        userSelectListItems.Add(new SelectListItem
        //        {
        //            Value = user?.Email?.ToString(),
        //            Text = user?.Id
        //        });
        //    }

        //    return View(new DetailModel
        //    {
        //        IdDiscount = id,
        //        FullName = discount.Buyer.UserExtraInfo.FullName,
        //        Email = discount.Buyer.Email,
        //        Voucher = discount.Voucher,
        //        ReductionPercentage = discount.ReductionPercentage,
        //        IsUsed = discount.IsUsed
        //    });
        //}

    //    [HttpGet]
    //    public async Task<IActionResult> Create()
    //    {
    //        return await LoadCreateModel();
    //    }
    //    [HttpPost]
    //    public async Task<IActionResult> Create(CreateModel createModel)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return await LoadCreateModel(createModel);
    //        }

    //        var postCategories = new List<PostCategory>();
    //        foreach (var categoryId in createModel.CategoryIds)
    //        {
    //            postCategories.Add(new PostCategory
    //            {
    //                CategoryId = categoryId
    //            });
    //        }

    //        var postTags = new List<PostTag>();
    //        foreach (var tagId in createModel.TagIds)
    //        {
    //            postTags.Add(new PostTag
    //            {
    //                TagId = tagId
    //            });
    //        }

    //        var uploadedImage = HttpContext.Request.Form.Files.FirstOrDefault();
    //        var baseImageName = string.Empty;
    //        if (uploadedImage != null)
    //        {
    //            baseImageName = Guid.NewGuid().ToString() + ".webp";
    //        }

    //        _onlineShopDbContext.Add(new Post
    //        {
    //            Title = createModel.Title,
    //            Body = createModel.Body,
    //            Introduction = createModel.Introduction,
    //            PublicationDateTime = DateTime.Now,
    //            ModificationDateTime = null,
    //            IsPublishable = createModel.IsPublishable,
    //            ImageName = baseImageName,
    //            PostTags = postTags,
    //            PostCategories = postCategories,
    //            BloggerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
    //        });

    //        await _onlineShopDbContext.SaveChangesAsync();

    //        if (uploadedImage != null)
    //        {
    //            var uploadedPostsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "posts");

    //            var originalImagePath = Path.Combine(uploadedPostsPath, baseImageName);
    //            var originalBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 800, 450);
    //            await ImageHelper.ConvertToWebP(originalImagePath, originalBitmap);

    //            var thumbnailImagePath = Path.Combine(uploadedPostsPath, "sm-" + baseImageName);
    //            var thumbnailBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 70, 39);
    //            await ImageHelper.ConvertToWebP(thumbnailImagePath, thumbnailBitmap);
    //        }

    //        return RedirectToAction("Index", "Post");
    //    }
    //    [HttpGet]
    //    public async Task<IActionResult> Update(int id)
    //    {
    //        return await LoadUpdateModel(id);
    //    }
    //    [HttpPost]
    //    public async Task<IActionResult> Update(UpdateModel updateModel)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return await LoadUpdateModel(updateModel.IdPost, updateModel);
    //        }

    //        var post = await _onlineShopDbContext.Posts
    //            .Include(p => p.Comments)
    //            .Include(p => p.PostCategories)
    //            .Include(p => p.PostTags)
    //            .FirstOrDefaultAsync(p => p.IdPost == updateModel.IdPost);
    //        if (post == null)
    //        {
    //            return RedirectToAction("Index", "Post");
    //        }

    //        var firstNotSecondCategoryIds = post.PostCategories.Select(pc => pc.CategoryId).ToList()
    //            .Except(updateModel.CategoryIds).ToList();
    //        var secondNotFirstCategoryIds = updateModel.CategoryIds
    //            .Except(post.PostCategories.Select(pc => pc.CategoryId)).ToList();
    //        if (firstNotSecondCategoryIds.Any() || secondNotFirstCategoryIds.Any())
    //        {
    //            var linkedPostCategories = _onlineShopDbContext.PostCategories
    //                .Where(pc => pc.PostId == updateModel.IdPost);
    //            _onlineShopDbContext.RemoveRange(linkedPostCategories);
    //            var postCategories = new List<PostCategory>();
    //            foreach (var categoryId in updateModel.CategoryIds)
    //            {
    //                postCategories.Add(new PostCategory
    //                {
    //                    PostId = post.IdPost,
    //                    CategoryId = categoryId
    //                });
    //            }
    //            _onlineShopDbContext.AddRange(postCategories);
    //        }

    //        var firstNotSecondTagIds = post.PostTags.Select(pc => pc.TagId).ToList()
    //                .Except(updateModel.TagIds).ToList();
    //        var secondNotFirstTagIds = updateModel.TagIds
    //                .Except(post.PostTags.Select(pc => pc.TagId)).ToList();
    //        if (firstNotSecondTagIds.Any() || secondNotFirstTagIds.Any())
    //        {
    //            var linkedPostTags = _onlineShopDbContext.PostTags
    //                .Where(pc => pc.PostId == updateModel.IdPost);
    //            _onlineShopDbContext.RemoveRange(linkedPostTags);
    //            var postTags = new List<PostTag>();
    //            foreach (var TagId in updateModel.TagIds)
    //            {
    //                postTags.Add(new PostTag
    //                {
    //                    PostId = post.IdPost,
    //                    TagId = TagId
    //                });
    //            }
    //            _onlineShopDbContext.AddRange(postTags);
    //        }

    //        string previousImageName = post.ImageName;
    //        var uploadedImage = HttpContext.Request.Form.Files.FirstOrDefault();
    //        string baseImageName = string.Empty;
    //        if (uploadedImage != null)
    //        {
    //            post.ImageName = baseImageName = Guid.NewGuid().ToString() + ".webp";
    //        }

    //        post.Title = updateModel.Title;
    //        post.Body = updateModel.Body;
    //        post.Introduction = updateModel.Introduction;
    //        post.IsPublishable = updateModel.IsPublishable;
    //        post.ModificationDateTime = DateTime.Now;

    //        await _onlineShopDbContext.SaveChangesAsync();

    //        if (uploadedImage != null)
    //        {
    //            var uploadedPostsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "posts");

    //            var previousOriginalImagePath =
    //                Path.Combine(uploadedPostsPath, previousImageName);
    //            var file = new FileInfo(previousOriginalImagePath);
    //            if (file.Exists)
    //            {
    //                file.Delete();
    //            }
    //            var originalImagePath = Path.Combine(uploadedPostsPath, baseImageName);
    //            var originalBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 800, 450);
    //            await ImageHelper.ConvertToWebP(originalImagePath, originalBitmap);

    //            var previousThumbnailImagePath =
    //                Path.Combine(uploadedPostsPath, "sm-" + previousImageName);
    //            file = new FileInfo(previousThumbnailImagePath);
    //            if (file.Exists)
    //            {
    //                file.Delete();
    //            }
    //            var thumbnailImagePath = Path.Combine(uploadedPostsPath, "sm-" + baseImageName);
    //            var thumbnailBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 70, 39);
    //            await ImageHelper.ConvertToWebP(thumbnailImagePath, thumbnailBitmap);
    //        }

    //        return RedirectToAction("Index", "Post");
    //    }
    //    [HttpGet]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        var post = await _onlineShopDbContext.Posts
    //           .Include(p => p.Comments)
    //           .Include(p => p.PostCategories)
    //           .Include(p => p.PostTags)
    //           .FirstOrDefaultAsync(t => t.IdPost == id);

    //        if (post == null)
    //        {
    //            return RedirectToAction("Index", "Post");
    //        }

    //        _onlineShopDbContext.Comments.RemoveRange(post.Comments);
    //        _onlineShopDbContext.PostCategories.RemoveRange(post.PostCategories);
    //        _onlineShopDbContext.PostTags.RemoveRange(post.PostTags);
    //        _onlineShopDbContext.Posts.Remove(post);

    //        await _onlineShopDbContext.SaveChangesAsync();

    //        var uploadedPostsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "posts");
    //        var previousOriginalImagePath =
    //            Path.Combine(uploadedPostsPath, post.ImageName);
    //        var file = new FileInfo(previousOriginalImagePath);
    //        if (file.Exists)
    //        {
    //            file.Delete();
    //        }

    //        var previousThumbnailImagePath =
    //            Path.Combine(uploadedPostsPath, "sm-" + post.ImageName);
    //        file = new FileInfo(previousThumbnailImagePath);
    //        if (file.Exists)
    //        {
    //            file.Delete();
    //        }

    //        return RedirectToAction("Index", "Post");
    //    }
    //    public async Task<IActionResult> LoadUpdateModel
    //        (int id, UpdateModel previousUpdateModel = null)
    //    {
    //        var post = await _onlineShopDbContext.Posts
    //            .Include(p => p.Comments)
    //                .ThenInclude(t => t.AspNetUser)
    //                    .ThenInclude(t => t.UserExtraInfo)
    //            .Include(p => p.PostCategories)
    //            .Include(p => p.PostTags)
    //            .FirstOrDefaultAsync(p => p.IdPost == id);

    //        if (post == null)
    //        {
    //            return RedirectToAction("Index", "Post");
    //        }

    //        var categories = await _onlineShopDbContext.Categories.ToListAsync();
    //        var categorySelectListItems = new List<SelectListItem>();
    //        foreach (var category in categories)
    //        {
    //            categorySelectListItems.Add(new SelectListItem
    //            {
    //                Value = category.IdCategory.ToString(),
    //                Text = category.Title
    //            });
    //        }

    //        var tags = await _onlineShopDbContext.Tags.ToListAsync();
    //        var tagSelectListItems = new List<SelectListItem>();
    //        foreach (var tag in tags)
    //        {
    //            tagSelectListItems.Add(new SelectListItem
    //            {
    //                Value = tag.IdTag.ToString(),
    //                Text = tag.Title
    //            });
    //        }

    //        Dictionary<Guid, List<CommentModel>> branchCommentModels =
    //            new Dictionary<Guid, List<CommentModel>>();
    //        IEnumerable<IGrouping<Guid, Comment>> commentBranchs =
    //            post.Comments.GroupBy(t => t.CommentBranchId);

    //        foreach (var commentBranch in commentBranchs)
    //        {
    //            var branchHeadComment = commentBranch.OrderBy
    //                (t => t.CreationDateTime).FirstOrDefault();

    //            var commentModels = new List<CommentModel>();
    //            foreach (var comment in commentBranch.ToList())
    //            {
    //                commentModels.Add(
    //                    new CommentModel
    //                    {
    //                        IdComment = comment.IdComment,
    //                        IsBranchHead = comment.IdComment == branchHeadComment.IdComment ? true : false,
    //                        CommentBranchId = comment.CommentBranchId,
    //                        FullName = comment.CommenterId == null ?
    //                            comment.FullName :
    //                            comment.AspNetUser.UserExtraInfo.FullName,
    //                        Body = comment.Body,
    //                        CreationDateTime = comment.CreationDateTime,
    //                        AvatarName = comment.CommenterId == null ?
    //                            string.Empty :
    //                            comment.AspNetUser.UserExtraInfo.AvatarName,
    //                        PostId = comment.PostId,
    //                        IsPublishable = comment.IsPublishable,
    //                        IsReviewed = comment.IsReviewed
    //                    }
    //                );
    //            }
    //            branchCommentModels.Add
    //            (
    //                commentBranch.Key,
    //                commentModels
    //            );
    //        }

    //        int lastCommentId = default;
    //        if (branchCommentModels.Any())
    //        {
    //            var lastBranchComment = branchCommentModels.LastOrDefault();
    //            lastCommentId = lastBranchComment.Value.LastOrDefault().IdComment;
    //        }

    //        if (previousUpdateModel != null)
    //        {
    //            var updateModel = new UpdateModel
    //            {
    //                IdPost = previousUpdateModel.IdPost,
    //                Title = previousUpdateModel?.Title,
    //                Body = previousUpdateModel?.Body,
    //                Introduction = previousUpdateModel.Introduction,
    //                CategoryIds = previousUpdateModel?.CategoryIds,
    //                TagIds = previousUpdateModel?.TagIds,
    //                Image = previousUpdateModel?.Image,
    //                IsPublishable = previousUpdateModel == null ?
    //                    false : previousUpdateModel.IsPublishable,
    //                CategorySelectListItems = categorySelectListItems,
    //                TagSelectListItems = tagSelectListItems,
    //                ImageName = post.ImageName,
    //                BranchComments = branchCommentModels,
    //                LastCommentId = lastCommentId,
    //            };

    //            return View(updateModel);
    //        }
    //        return View(new UpdateModel
    //        {
    //            IdPost = id,
    //            Title = post.Title,
    //            Body = post.Body,
    //            Introduction = post.Introduction,
    //            CategoryIds = post.Categories.Select(t => t.IdCategory).ToList(),
    //            TagIds = post.Tags.Select(t => t.IdTag).ToList(),
    //            IsPublishable = post.IsPublishable,
    //            CategorySelectListItems = categorySelectListItems,
    //            TagSelectListItems = tagSelectListItems,
    //            ImageName = post.ImageName,
    //            BranchComments = branchCommentModels,
    //            LastCommentId = lastCommentId,
    //        });
    //    }
    //    public async Task<IActionResult> LoadCreateModel
    //        (CreateModel previousCreateModel = null)
    //    {
    //        var categories = await _onlineShopDbContext.Categories.ToListAsync();
    //        var categorySelectListItems = new List<SelectListItem>();
    //        foreach (var category in categories)
    //        {
    //            categorySelectListItems.Add(new SelectListItem
    //            {
    //                Value = category.IdCategory.ToString(),
    //                Text = category.Title
    //            });
    //        }

    //        var tags = await _onlineShopDbContext.Tags.ToListAsync();
    //        var tagSelectListItems = new List<SelectListItem>();
    //        foreach (var tag in tags)
    //        {
    //            tagSelectListItems.Add(new SelectListItem
    //            {
    //                Value = tag.IdTag.ToString(),
    //                Text = tag.Title
    //            });
    //        }

    //        var createModel = new CreateModel
    //        {
    //            Title = previousCreateModel?.Title,
    //            Body = previousCreateModel?.Body,
    //            Introduction = previousCreateModel?.Introduction,
    //            CategoryIds = previousCreateModel?.CategoryIds,
    //            TagIds = previousCreateModel?.TagIds,
    //            Image = previousCreateModel?.Image,
    //            IsPublishable = previousCreateModel == null ?
    //                false : previousCreateModel.IsPublishable,
    //            CategorySelectListItems = categorySelectListItems,
    //            TagSelectListItems = tagSelectListItems,
    //        };

    //        return View(createModel);
    //    }
    }
}