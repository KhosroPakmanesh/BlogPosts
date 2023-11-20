using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.Shipping;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ShippingController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public ShippingController(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetShippings()
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

            var queryableShippings = _onlineShopDbContext.Shippings
                .Include(t=>t.Order)
                    .ThenInclude(t=>t.Buyer)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableShippings = queryableShippings.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    queryableShippings = queryableShippings.Where(
            //        m => m.Voucher.Contains(searchValue));
            //}

            var rawShippings = await queryableShippings
                .Skip(skip).Take(pageSize)
                .ToListAsync();
            recordsTotal = rawShippings.Count();

            var formattedShippings = new List<ShippingModel>();
            foreach (var rawShipping in rawShippings)
            {
                formattedShippings.Add(new ShippingModel
                {
                    IdShipping = rawShipping.IdShipping,
                    OrderId=rawShipping.OrderId,
                    BuyerUserName = rawShipping?.Order?.Buyer?.UserName!,
                    IsShipped = rawShipping?.IsShipped ?? false
                });
            }

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = formattedShippings
            };

            return Ok(responseObject);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var shipping = await _onlineShopDbContext.Shippings
                .Include(t => t.Order)
                    .ThenInclude(t => t.Buyer)
                .FirstOrDefaultAsync(p => p.IdShipping == id);

            if (shipping == null)
            {
                return RedirectToAction("Index", "Shipping");
            }

            return View(new DetailModel
            {
                OrderId = shipping.OrderId,
                BuyerUserName = shipping?.Order?.Buyer?.UserName!,
                IsShipped = shipping?.IsShipped ?? false
            });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            return await LoadUpdateModel(id);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoadUpdateModel(updateModel.IdShipping, updateModel);
            }

            var shipping = await _onlineShopDbContext.Shippings
                .Include(t => t.Order)
                    .ThenInclude(t => t.Buyer)
                .FirstOrDefaultAsync(p => p.IdShipping == updateModel.IdShipping);
            if (shipping == null)
            {
                return RedirectToAction("Index", "Shipping");
            }

            shipping.IsShipped = updateModel.IsShipped;

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Shipping");
        }
        public async Task<IActionResult> LoadUpdateModel
            (int id, UpdateModel previousUpdateModel = null!)
        {
            var shipping = await _onlineShopDbContext.Shippings
                .Include(t => t.Order)
                    .ThenInclude(t => t.Buyer)
                .FirstOrDefaultAsync(p => p.IdShipping == id);

            if (shipping == null)
            {
                return RedirectToAction("Index", "Shipping");
            }

            if (previousUpdateModel != null)
            {
                var updateModel = new UpdateModel
                {
                    IdShipping = previousUpdateModel.IdShipping,
                    OrderId = previousUpdateModel.OrderId,
                    BuyerUserName = previousUpdateModel.BuyerUserName,
                    IsShipped = previousUpdateModel.IsShipped
                };

                return View(updateModel);
            }

            return View(new UpdateModel
            {
                IdShipping = shipping.IdShipping,
                OrderId = shipping.OrderId,
                BuyerUserName = shipping.Order?.Buyer?.UserName!,
                IsShipped = shipping.IsShipped,
            });
        }
    }
}