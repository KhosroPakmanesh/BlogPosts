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
//using Microsoft.AspNetCore.Identity;
//using System.Threading.Tasks;
//using System.Linq.Dynamic.Core;
//using Website.Presentation.Data;
//using Website.Presentation.Entities;
//using Website.Presentation.Areas.Admin.Models.Service;
//using System.Security.Claims;
//using Website.Presentation.Helpers;
//using Microsoft.Extensions.Hosting;

//namespace Website.Presentation.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Roles = "admin")]
//    public class ServiceController : Controller
//    {
//        private readonly IWebHostEnvironment webHostEnvironment;
//        private readonly BlogDbContext _blogDbContext;

//        public ServiceController(IWebHostEnvironment webHostEnvironment,
//            BlogDbContext blogDbContext){
//            this.webHostEnvironment = webHostEnvironment;
//            _blogDbContext = blogDbContext;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> GetServices()
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

//                var expectedServices = _blogDbContext.Services.AsQueryable();

//                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
//                {
//                    expectedServices = expectedServices.OrderBy(sortColumn + " " + sortColumnDirection);
//                }

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    expectedServices = expectedServices.Where(
//                        m => m.Title.Contains(searchValue));
//                }

//                recordsTotal = await expectedServices.CountAsync();
//                var retrievedServices = await expectedServices.Skip(skip).Take(pageSize).
//                    Select(t =>
//                    new
//                    {
//                        t.IdService,
//                        t.Title,
//                        t.ImportanceOrder,
//                        t.IsPublishable,
//                    }).ToListAsync();

//                var responseObject = new
//                {
//                    draw,
//                    recordsFiltered = recordsTotal,
//                    recordsTotal,
//                    data = retrievedServices
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
//            var service = await _blogDbContext.Services
//                .Include(p => p.ServiceCategories)
//                .Include(p => p.ServiceTags)
//                .FirstOrDefaultAsync(p => p.IdService == id);

//            if (service == null)
//            {
//                return RedirectToAction("Index", "Service");
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

//            var cultureInfo = new CultureInfo("en-us");
//            return View(new DetailModel
//            {
//                IdService = id,
//                Title = service.Title,
//                TenCharacterSummary=service.TenCharacterSummary,
//                ThreeBulletPoints= service.ThreeBulletPoints,
//                Body = service.Body,
//                CategoryIds = service.Categories.Select(t => t.IdCategory).ToList(),
//                CategorySelectListItems = categorySelectListItems,
//                TagIds = service.Tags.Select(t => t.IdTag).ToList(),
//                TagSelectListItems = tagSelectListItems,
//                IsPublishable = service.IsPublishable,
//                ImportanceOrder = service.ImportanceOrder,
//                ImageName = service.ImageName
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

//            var serviceCategories = new List<ServiceCategory>();
//            foreach (var categoryId in createModel.CategoryIds)
//            {
//                serviceCategories.Add(new ServiceCategory
//                {
//                    CategoryId = categoryId
//                });
//            }

//            var serviceTags = new List<ServiceTag>();
//            foreach (var tagId in createModel.TagIds)
//            {
//                serviceTags.Add(new ServiceTag
//                {
//                    TagId = tagId
//                });
//            }

//            var uploadedImage = HttpContext.Request.Form.Files.FirstOrDefault();
//            var baseImageName = string.Empty;
//            if (uploadedImage != null)
//            {
//                baseImageName = baseImageName = Guid.NewGuid().ToString() + ".webp";
//            }
                        
//            _blogDbContext.Services.Add(new Entities.Service
//            {
//                Title = createModel.Title,
//                TenCharacterSummary = createModel.TenCharacterSummary,
//                ThreeBulletPoints = createModel.ThreeBulletPoints,
//                Body = createModel.Body,
//                ImageName = baseImageName,
//                ServiceTags = serviceTags,
//                ServiceCategories = serviceCategories,
//                IsPublishable = createModel.IsPublishable,
//                ImportanceOrder = createModel.ImportanceOrder,
//                ServiceProviderId= User.FindFirstValue(ClaimTypes.NameIdentifier)
//            });

//            await _blogDbContext.SaveChangesAsync();

//            if (uploadedImage != null)
//            {
//                var uploadedPostsPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "services");

//                var originalImagePath = Path.Combine(uploadedPostsPath, baseImageName);
//                var originalBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 1920, 1080);
//                await ImageHelper.ConvertToWebP(originalImagePath, originalBitmap);

//                var thumbnailImagePath = Path.Combine(uploadedPostsPath, "sm-" + baseImageName);
//                var thumbnailBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 800, 450);
//                await ImageHelper.ConvertToWebP(thumbnailImagePath, thumbnailBitmap);
//            }

//            return RedirectToAction("Index", "Service");
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
//                return await LoadUpdateModel(updateModel.IdService, updateModel);
//            }

//            var service = await _blogDbContext.Services
//                .Include(p => p.ServiceCategories)
//                .Include(p => p.ServiceTags)
//                .FirstOrDefaultAsync(p => p.IdService == updateModel.IdService);
//            if (service == null)
//            {
//                return RedirectToAction("Index", "Service");
//            }

//            var firstNotSecondCategoryIds = service.ServiceCategories
//                .Select(pc => pc.CategoryId).ToList()
//                .Except(updateModel.CategoryIds).ToList();
//            var secondNotFirstCategoryIds = updateModel.CategoryIds
//                .Except(service.ServiceCategories
//                .Select(pc => pc.CategoryId)).ToList();
//            if (firstNotSecondCategoryIds.Any() || secondNotFirstCategoryIds.Any())
//            {
//                var linkedServiceCategories = _blogDbContext.ServiceCategories
//                    .Where(pc => pc.ServiceId == updateModel.IdService);
//                _blogDbContext.RemoveRange(linkedServiceCategories);
//                var serviceCategories = new List<ServiceCategory>();
//                foreach (var categoryId in updateModel.CategoryIds)
//                {
//                    serviceCategories.Add(new ServiceCategory
//                    {
//                        ServiceId = service.IdService,
//                        CategoryId = categoryId
//                    });
//                }
//                _blogDbContext.AddRange(serviceCategories);
//            }

//            var firstNotSecondTagIds = service.ServiceTags
//                .Select(pc => pc.TagId).ToList()
//                .Except(updateModel.TagIds).ToList();
//            var secondNotFirstTagIds = updateModel.TagIds
//                .Except(service.ServiceTags
//                .Select(pc => pc.TagId)).ToList();
//            if (firstNotSecondTagIds.Any() || secondNotFirstTagIds.Any())
//            {
//                var linkedServiceTags = _blogDbContext.ServiceTags
//                    .Where(pc => pc.ServiceId == updateModel.IdService);
//                _blogDbContext.RemoveRange(linkedServiceTags);
//                var serviceTags = new List<ServiceTag>();
//                foreach (var TagId in updateModel.TagIds)
//                {
//                    serviceTags.Add(new ServiceTag
//                    {
//                        ServiceId = service.IdService,
//                        TagId = TagId
//                    });
//                }
//                _blogDbContext.AddRange(serviceTags);
//            }

//            string previousImageName = service.ImageName;
//            var uploadedImage = HttpContext.Request.Form.Files.FirstOrDefault();
//            string baseImageName = string.Empty;
//            if (uploadedImage != null)
//            {                
//                service.ImageName = baseImageName = Guid.NewGuid().ToString() + ".webp";
//            }

//            service.Title = updateModel.Title;
//            service.TenCharacterSummary = updateModel.TenCharacterSummary;
//            service.ThreeBulletPoints = updateModel.ThreeBulletPoints;
//            service.Body = updateModel.Body;
//            service.IsPublishable = updateModel.IsPublishable;
//            service.ImportanceOrder = updateModel.ImportanceOrder;

//            await _blogDbContext.SaveChangesAsync();

//            if (uploadedImage != null)
//            {
//                var uploadedPostsPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "services");

//                var previousOriginalImagePath =
//                    Path.Combine(uploadedPostsPath, previousImageName);
//                var file = new FileInfo(previousOriginalImagePath);
//                if (file.Exists)
//                {
//                    file.Delete();
//                }
//                var originalImagePath = Path.Combine(uploadedPostsPath, baseImageName);
//                var originalBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 1920, 1080);
//                await ImageHelper.ConvertToWebP(originalImagePath, originalBitmap);

//                var previousThumbnailImagePath =
//                    Path.Combine(uploadedPostsPath, "sm-" + previousImageName);
//                file = new FileInfo(previousThumbnailImagePath);
//                if (file.Exists)
//                {
//                    file.Delete();
//                }
//                var thumbnailImagePath = Path.Combine(uploadedPostsPath, "sm-" + baseImageName);
//                var thumbnailBitmap = ImageHelper.ResizeImage(uploadedImage.OpenReadStream(), 800, 450);
//                await ImageHelper.ConvertToWebP(thumbnailImagePath, thumbnailBitmap);
//            }

//            return RedirectToAction("Index", "Service");
//        }
//        [HttpGet]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var service = await _blogDbContext.Services
//               .Include(p => p.ServiceCategories)
//               .Include(p => p.ServiceTags)
//               .FirstOrDefaultAsync(t => t.IdService == id);

//            if (service == null)
//            {
//                return RedirectToAction("Index", "Service");
//            }

//            _blogDbContext.ServiceCategories.RemoveRange(service.ServiceCategories);
//            _blogDbContext.ServiceTags.RemoveRange(service.ServiceTags);
//            _blogDbContext.Services.Remove(service);

//            await _blogDbContext.SaveChangesAsync();

//            var uploadedPostsPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "services");
//            var previousOriginalImagePath =
//                Path.Combine(uploadedPostsPath, service.ImageName);
//            var file = new FileInfo(previousOriginalImagePath);
//            if (file.Exists)
//            {
//                file.Delete();
//            }
//            var previousThumbnailImagePath =
//                Path.Combine(uploadedPostsPath, "sm-" + service.ImageName);
//            file = new FileInfo(previousThumbnailImagePath);
//            if (file.Exists)
//            {
//                file.Delete();
//            }

//            return RedirectToAction("Index", "Service");
//        }
//        public async Task<IActionResult> LoadUpdateModel(int id, UpdateModel previousUpdateModel = null)
//        {
//            var service = await _blogDbContext.Services
//                .Include(p => p.ServiceCategories)
//                .Include(p => p.ServiceTags)
//                .FirstOrDefaultAsync(p => p.IdService == id);

//            if (service == null)
//            {
//                return RedirectToAction("Index", "Service");
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

//            var tags =await _blogDbContext.Tags.ToListAsync();
//            var tagSelectListItems = new List<SelectListItem>();
//            foreach (var tag in tags)
//            {
//                tagSelectListItems.Add(new SelectListItem
//                {
//                    Value = tag.IdTag.ToString(),
//                    Text = tag.Title
//                });
//            }

//            if (previousUpdateModel != null)
//            {
//                var updateModel = new UpdateModel
//                {
//                    IdService = previousUpdateModel.IdService,
//                    TenCharacterSummary=previousUpdateModel?.TenCharacterSummary,
//                    ThreeBulletPoints = previousUpdateModel?.ThreeBulletPoints,  
//                    Title = previousUpdateModel?.Title,
//                    Body = previousUpdateModel?.Body,
//                    CategoryIds = previousUpdateModel?.CategoryIds,
//                    TagIds = previousUpdateModel?.TagIds,
//                    Image = previousUpdateModel?.Image,
//                    CategorySelectListItems = categorySelectListItems,
//                    TagSelectListItems = tagSelectListItems,
//                    IsPublishable = previousUpdateModel == null ?
//                        false : previousUpdateModel.IsPublishable,
//                    ImportanceOrder = previousUpdateModel == null ?
//                        0 : previousUpdateModel.ImportanceOrder,
//                    ImageName = service.ImageName 
//                };

//                return View(updateModel);
//            }
//            return View(new UpdateModel
//            {
//                IdService = id,
//                Title = service.Title,
//                TenCharacterSummary = service.TenCharacterSummary,
//                ThreeBulletPoints = service.ThreeBulletPoints,
//                Body = service.Body,
//                CategoryIds = service.Categories.Select(t => t.IdCategory).ToList(),
//                TagIds = service.Tags.Select(t => t.IdTag).ToList(),
//                CategorySelectListItems = categorySelectListItems,
//                TagSelectListItems = tagSelectListItems,
//                IsPublishable = service.IsPublishable,
//                ImportanceOrder = service.ImportanceOrder,
//                ImageName = service.ImageName 
//            });
//        }
//        public async Task<IActionResult> LoadCreateModel(CreateModel previousCreateModel = null)
//        {
//            var categories =await _blogDbContext.Categories.ToListAsync();
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
//                TenCharacterSummary = previousCreateModel?.TenCharacterSummary,
//                ThreeBulletPoints = previousCreateModel?.ThreeBulletPoints,
//                Body = previousCreateModel?.Body,
//                CategoryIds = previousCreateModel?.CategoryIds,
//                TagIds = previousCreateModel?.TagIds,
//                Image = previousCreateModel?.Image,
//                CategorySelectListItems = categorySelectListItems,
//                IsPublishable = previousCreateModel == null ?
//                    false : previousCreateModel.IsPublishable,
//                ImportanceOrder = previousCreateModel == null ?
//                        0 : previousCreateModel.ImportanceOrder,
//                TagSelectListItems = tagSelectListItems,
//            };

//            return View(createModel);
//        }
//    }
//}
