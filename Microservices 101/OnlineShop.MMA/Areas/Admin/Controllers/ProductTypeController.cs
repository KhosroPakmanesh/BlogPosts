using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Areas.Admin.Models.ProductType;
using OnlineShop.MMA.Data.OnlineShopDbContext;


namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ProductTypeController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public ProductTypeController(
            OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetProductTypes()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var queryableProductTypes = _onlineShopDbContext.ProductTypes.AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableProductTypes = queryableProductTypes.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    queryableProductTypes = queryableProductTypes.Where(
            //        m => m.Name.Contains(searchValue));
            //}

            recordsTotal = await queryableProductTypes.CountAsync();
            var retrievedProductTypes = await queryableProductTypes.Skip(skip).Take(pageSize).
                Select(t =>
                new ProductTypeModel
                {
                    IdProductType=t.IdProductType,
                    Name=t.Name,
                    Description=t.Description!
                }).ToListAsync();

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = retrievedProductTypes
            };

            return Ok(responseObject);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var productType = await _onlineShopDbContext.ProductTypes
                .FirstOrDefaultAsync(t => t.IdProductType == id);

            if (productType == null)
            {
                return RedirectToAction("Index", "ProductType");
            }

            return View(new DetailModel
            {
                Name = productType.Name,
                Description = productType.Description!
            });
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel createModel)
        {
            var existingProductType = await _onlineShopDbContext.ProductTypes.
                FirstOrDefaultAsync(t => t.Name == createModel.Name);
            if (existingProductType != null)
            {
                ModelState.AddModelError
                    (string.Empty, "There can be only one product type with a certain name.");
            }

            if (!ModelState.IsValid)
            {
                return View(createModel);
            }

            _onlineShopDbContext.ProductTypes.Add(new ProductType
            {
                Name = createModel.Name,
                Description = createModel.Description
            });

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "ProductType");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            return await LoadUpdateModel(id);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateModel updateModel)
        {
            var existingProductType = await _onlineShopDbContext.ProductTypes
                .Where(t => t.IdProductType != updateModel.IdProductType)
                .Where(t => t.Name == updateModel.Name)
                .FirstOrDefaultAsync();
            if (existingProductType != null)
            {
                ModelState.AddModelError
                    (string.Empty, "There can be only one product type with a certain name.");
            }

            if (!ModelState.IsValid)
            {
                return await LoadUpdateModel(updateModel.IdProductType, updateModel);
            }

            var productType = await _onlineShopDbContext.ProductTypes
                .FirstOrDefaultAsync(t => t.IdProductType == updateModel.IdProductType);

            if (productType == null)
            {
                return RedirectToAction("Index", "ProductType");
            }

            productType.Name = updateModel.Name;
            productType.Description = updateModel.Description;
            _onlineShopDbContext.ProductTypes.Update(productType);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "ProductType");
        }
        public async Task<IActionResult> 
            LoadUpdateModel(int productTypeId, UpdateModel previousUpdateModel= null)
        {
            var productType =await _onlineShopDbContext.ProductTypes
                .FirstOrDefaultAsync(t => t.IdProductType == productTypeId);

            if (productType == null)
            {
                return RedirectToAction("Index", "ProductType");
            }

            if (previousUpdateModel != null)
            {
                return View(new UpdateModel
                {
                    IdProductType = previousUpdateModel.IdProductType,
                    Name = previousUpdateModel.Name,
                    Description = previousUpdateModel.Description
                });
            }

            return View(new UpdateModel
            {
                IdProductType = productTypeId,
                Name = productType.Name,
                Description = productType.Description!
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var productType = await _onlineShopDbContext.ProductTypes
                .Include(t => t.Products)
                .FirstOrDefaultAsync(t => t.IdProductType == id);

            if (productType == null)
            {
                return RedirectToAction("Index", "ProductType");
            }

            if (productType.Products.Any())
            {
                ModelState.AddModelError("Error", "There are some posts associated with this tag. " +
                    "You can delete a tag when there is no post associated with it.");
                return RedirectToAction("Index", "ProductType");
            }

            _onlineShopDbContext.ProductTypes.Remove(productType);
            await _onlineShopDbContext.SaveChangesAsync(); ;

            return RedirectToAction("Index", "ProductType");
        }
    }
}
