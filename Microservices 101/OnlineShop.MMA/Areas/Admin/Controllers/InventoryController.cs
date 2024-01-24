using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.MMA.Areas.Admin.Models.Inventory;
using Microsoft.CodeAnalysis;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class InventoryController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public InventoryController(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetInventories()
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

            var queryableInventories = _onlineShopDbContext.Inventories
                .Include(t=>t.Product)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableInventories = queryableInventories.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    expectedInventories = expectedInventories.Where(
            //        m => m.Quantity.Contains(searchValue));
            //}

            recordsTotal = await queryableInventories.CountAsync();
            var rawInventories = await queryableInventories
                .Skip(skip).Take(pageSize)
                .ToListAsync();

            var formattedInventories = new List<InventoryModel>();
            foreach (var rawInventory in rawInventories)
            {
                formattedInventories.Add(new InventoryModel
                {
                    IdInventory=rawInventory.IdInventory,
                    ProductId = rawInventory.Product.IdProduct,
                    ProductName=rawInventory.Product.Name,
                    Quantity =rawInventory.Quantity,
                });
            }

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = formattedInventories
            };

            return Ok(responseObject);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var inventory = await _onlineShopDbContext.Inventories
                .Include(t => t.Product)
                .FirstOrDefaultAsync(p => p.IdInventory == id);

            if (inventory == null)
            {
                return RedirectToAction("Index", "Inventory");
            }

            return View(new DetailModel
            {
                ProductName = inventory.Product.Name,
                Quantity = inventory.Quantity
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
            var existingInventory = await _onlineShopDbContext.Inventories.
                FirstOrDefaultAsync(t => t.ProductId == createModel.ProductId);
            if (existingInventory != null)
            {
                ModelState.AddModelError
                    (string.Empty, "There can be only one inventory for each product.");
            }

            if (!ModelState.IsValid)
            {
                return await LoadCreateModel(createModel);
            }

            _onlineShopDbContext.Inventories.Add(new Inventory
            {
                ProductId = createModel.ProductId,
                Quantity = createModel.Quantity
            });

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Inventory");
        }
        public async Task<IActionResult> LoadCreateModel
            (CreateModel previousCreateModel = null!)
        {
            var products = await _onlineShopDbContext.Products.ToListAsync();
            var productSelectListItems = new List<SelectListItem>();
            foreach (var product in products)
            {
                productSelectListItems.Add(new SelectListItem
                {
                    Value = product?.IdProduct.ToString(),
                    Text = product?.Name
                });
            }

            var createModel = new CreateModel
            {
                ProductId = previousCreateModel?.ProductId ?? 0,
                Quantity = previousCreateModel?.Quantity ?? 0,
                ProductSelectListItems = productSelectListItems
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
            var existingInventory = await _onlineShopDbContext.Inventories
                .Where(t=>t.IdInventory != updateModel.IdInventory)
                .Where(t => t.ProductId == updateModel.ProductId)
                .FirstOrDefaultAsync();
            if (existingInventory != null)
            {
                ModelState.AddModelError
                    (string.Empty, "There can be only one inventory for each product.");
            }

            if (!ModelState.IsValid)
            {
                return await LoadUpdateModel(updateModel.IdInventory, updateModel);
            }

            var inventory = await _onlineShopDbContext.Inventories
                .FirstOrDefaultAsync(p => p.IdInventory == updateModel.IdInventory);

            if (inventory == null)
            {
                return RedirectToAction("Index", "Inventory");
            }

            inventory.ProductId = updateModel.ProductId;
            inventory.Quantity = updateModel.Quantity;

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Inventory");
        }
        public async Task<IActionResult> LoadUpdateModel
            (int id, UpdateModel previousUpdateModel = null!)
        {
            var inventory = await _onlineShopDbContext.Inventories
                .FirstOrDefaultAsync(p => p.IdInventory == id);

            if (inventory == null)
            {
                return RedirectToAction("Index", "Inventory");
            }

            var products = await _onlineShopDbContext.Products.ToListAsync();
            var productSelectListItems = new List<SelectListItem>();
            foreach (var product in products)
            {
                productSelectListItems.Add(new SelectListItem
                {
                    Value = product?.IdProduct.ToString(),
                    Text = product?.Name
                });
            }

            if (previousUpdateModel != null)
            {

                var updateModel = new UpdateModel
                {
                    IdInventory = previousUpdateModel.IdInventory,
                    Quantity = previousUpdateModel?.Quantity ?? 0,
                    ProductId = previousUpdateModel?.ProductId ?? 0,
                    ProductSelectListItems = productSelectListItems,
                };

                return View(updateModel);
            }

            return View(new UpdateModel
            {
                IdInventory = inventory.IdInventory,
                Quantity = inventory.Quantity,
                ProductId = inventory.ProductId,
                ProductSelectListItems = productSelectListItems
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _onlineShopDbContext.Inventories
                .FirstOrDefaultAsync(p => p.IdInventory == id);

            if (inventory == null)
            {
                return RedirectToAction("Index", "Inventory");
            }

            _onlineShopDbContext.Inventories.Remove(inventory);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Inventory");
        }
    }
}