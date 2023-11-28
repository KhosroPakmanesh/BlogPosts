using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using OnlineShop.MMA.Areas.Admin.Models.Product;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public ProductController(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetProducts()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"]
                .FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var queryableProducts = _onlineShopDbContext.Products
                .Include(t=>t.ProductType)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableProducts = queryableProducts.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    expectedInventories = expectedInventories.Where(
            //        m => m.Quantity.Contains(searchValue));
            //}

            var rawProducts = await queryableProducts
                .Skip(skip).Take(pageSize)
                .ToListAsync();
            recordsTotal = rawProducts.Count();

            var formattedProducts = new List<ProductModel>();
            foreach (var rawProduct in rawProducts)
            {
                formattedProducts.Add(new ProductModel
                {
                    IdProduct= rawProduct.IdProduct,
                    ProductTypeId= rawProduct.ProductTypeId,
                    ProductTypeName= rawProduct.ProductType.Name,
                    Name= rawProduct.Name,
                    Price= rawProduct.Price,
                    Description= rawProduct.Description!
                });
            }

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = formattedProducts
            };

            return Ok(responseObject);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _onlineShopDbContext.Products
                .Include(t => t.ProductType)
                .FirstOrDefaultAsync(p => p.IdProduct == id);

            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }

            return View(new DetailModel
            {
                ProductTypeName = product.ProductType.Name,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description!,
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return await LoadCreateModel();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel createModel)
        {
            var existingProduct = await _onlineShopDbContext.Products.
                FirstOrDefaultAsync(t => t.Name == createModel.Name);
            if (existingProduct != null)
            {
                ModelState.AddModelError
                    (string.Empty, "There can be only one product with a certain name.");
            }

            if (!ModelState.IsValid)
            {
                return await LoadCreateModel(createModel);
            }

            _onlineShopDbContext.Products.Add(new Product
            {
                ProductTypeId= createModel.ProductTypeId,
                Name= createModel.Name,
                Price= createModel.Price,
                Description= createModel.Description,
            });

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> LoadCreateModel
            (CreateModel previousCreateModel = null!)
        {
            var productTypes = await _onlineShopDbContext.ProductTypes.ToListAsync();
            var productTypesSelectListItems = new List<SelectListItem>();
            foreach (var productType in productTypes)
            {
                productTypesSelectListItems.Add(new SelectListItem
                {
                    Value = productType.IdProductType.ToString(),
                    Text = productType?.Name
                });
            }

            var createModel = new CreateModel
            { 
                ProductTypeId=previousCreateModel?.ProductTypeId ?? 0,
                Name = previousCreateModel?.Name!,
                Price= previousCreateModel?.Price ?? 0,
                Description= previousCreateModel?.Description!,
                ProductTypeSelectListItems = productTypesSelectListItems
            };

            return View(createModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            return await LoadUpdateModel(id);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateModel updateModel)
        {
            var existingProduct = await _onlineShopDbContext.Products
                .Where(t=>t.IdProduct != updateModel.IdProduct)
                .Where(t => t.Name == updateModel.Name)
                .FirstOrDefaultAsync();
            if (existingProduct != null)
            {
                ModelState.AddModelError
                    (string.Empty, "There can be only one product with a certain name.");
            }

            if (!ModelState.IsValid)
            {
                return await LoadUpdateModel(updateModel.IdProduct, updateModel);
            }

            var product = await _onlineShopDbContext.Products
                .FirstOrDefaultAsync(p => p.IdProduct == updateModel.IdProduct);

            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }

            product.ProductTypeId = updateModel.ProductTypeId;
            product.Name = updateModel.Name;
            product.Price = updateModel.Price;
            product.Description = updateModel.Description;

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> LoadUpdateModel
            (int id, UpdateModel previousUpdateModel = null!)
        {
            var product = await _onlineShopDbContext.Products
                .FirstOrDefaultAsync(p => p.IdProduct == id);

            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }

            var productTypes = await _onlineShopDbContext.ProductTypes.ToListAsync();
            var productTypesSelectListItems = new List<SelectListItem>();
            foreach (var productType in productTypes)
            {
                productTypesSelectListItems.Add(new SelectListItem
                {
                    Value = productType.IdProductType.ToString(),
                    Text = productType?.Name
                });
            }

            if (previousUpdateModel != null)
            {
                var updateModel = new UpdateModel
                {
                    IdProduct=id,
                    ProductTypeId = previousUpdateModel.ProductTypeId,
                    ProductTypeName = previousUpdateModel.ProductTypeName!,
                    Name = previousUpdateModel.Name!,
                    Price = previousUpdateModel.Price,
                    Description = previousUpdateModel.Description!,
                    ProductTypeSelectListItems = productTypesSelectListItems
                };

                return View(updateModel);
            }

            return View(new UpdateModel
            {
                IdProduct=id,
                ProductTypeId = product.ProductTypeId,
                ProductTypeName = product.ProductType.Name!,
                Name = product.Name!,
                Price = product.Price,
                Description = product.Description!,
                ProductTypeSelectListItems = productTypesSelectListItems
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _onlineShopDbContext.Products
                .FirstOrDefaultAsync(p => p.IdProduct == id);

            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }

            _onlineShopDbContext.Products.Remove(product);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Product");
        }
    }
}