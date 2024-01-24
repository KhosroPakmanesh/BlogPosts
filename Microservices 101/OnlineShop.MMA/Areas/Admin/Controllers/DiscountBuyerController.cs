using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.DiscountBuyer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class DiscountBuyerController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public DiscountBuyerController(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDiscountBuyers()
        {
            var parentId = Convert.ToInt32(Request.Form["parentId"].FirstOrDefault());
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

            var queryableDiscountBuyers = _onlineShopDbContext.DiscountBuyers
                .Include(t=>t.Buyer)
                .Where(t=>t.DiscountId== parentId)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableDiscountBuyers = queryableDiscountBuyers.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    queryableShippings = queryableShippings.Where(
            //        m => m.Voucher.Contains(searchValue));
            //}

            recordsTotal = await queryableDiscountBuyers.CountAsync();
            var rawDiscountBuyers = await queryableDiscountBuyers
                .Skip(skip).Take(pageSize)
                .ToListAsync();

            var formattedDiscountBuyers = new List<DiscountBuyerModel>();
            foreach (var rawDiscountBuyer in rawDiscountBuyers)
            {
                formattedDiscountBuyers.Add(new DiscountBuyerModel
                {
                    DiscountId = rawDiscountBuyer.DiscountId,
                    BuyerId=rawDiscountBuyer.BuyerId,
                    BuyerUserName = rawDiscountBuyer?.Buyer.UserName!,
                    IsUsed = rawDiscountBuyer?.IsUsed ?? false
                });
            }

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = formattedDiscountBuyers
            };

            return Ok(responseObject);
        }

        [HttpGet]
        [Route("Admin/Discount/{discountId:int}/DiscountBuyer/Detail/{buyerId}")]
        public async Task<IActionResult> Detail 
            (int discountId, string buyerId,
            [FromQuery(Name = "previousPage")] string previousPage)
        {
            var discountBuyer = await _onlineShopDbContext.DiscountBuyers
                .Include(t=>t.Buyer)
                .Where(t=>t.DiscountId == discountId)
                .Where(T=>T.BuyerId == buyerId)
                .FirstOrDefaultAsync();

            if (discountBuyer == null)
            {
                return RedirectToAction("Index", "Discount");
            }

            return View(new DetailModel
            {
                DiscountId= discountBuyer.DiscountId,
                //BuyerId = discountBuyer.BuyerId,
                BuyerUserName = discountBuyer?.Buyer?.UserName!,
                IsUsed = discountBuyer?.IsUsed ?? false,
                PreviousAction= previousPage 
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            return await LoadCreateModel(id);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoadCreateModel(createModel.DiscountId, createModel);
            }

            var discountBuyers = new List<DiscountBuyer>();
            foreach (var buyerId in createModel.BuyerIds)
            {
                discountBuyers.Add(new DiscountBuyer
                {
                    DiscountId= createModel.DiscountId,
                    BuyerId = buyerId,
                    IsUsed = createModel.IsUsed
                });
            }

            await _onlineShopDbContext.DiscountBuyers
                .AddRangeAsync(discountBuyers);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Update", "Discount",
                new { id = createModel.DiscountId });
        }
        public async Task<IActionResult> LoadCreateModel
            (int id, CreateModel previousCreateModel = null!)
        {
            var discount = await _onlineShopDbContext.Discounts
                .FirstOrDefaultAsync(p => p.IdDiscount == id);

            if (discount == null)
            {
                return RedirectToAction("Index", "Discount");
            }

            var buyers = await _onlineShopDbContext.AspNetUsers.ToListAsync();
            var buyerSelectListItems = new List<SelectListItem>();
            foreach (var buyer in buyers)
            {
                buyerSelectListItems.Add(new SelectListItem
                {
                    Value = buyer.Id,
                    Text = buyer.UserName
                });
            }

            var createModel = new CreateModel
            {
                DiscountId = id,
                BuyerIds = previousCreateModel?.BuyerIds!,
                IsUsed = previousCreateModel?.IsUsed ?? false,
                BuyerSelectListItems = buyerSelectListItems
            };

            return View(createModel);
        }

        [HttpGet]
        [Route("Admin/Discount/{discountId:int}/DiscountBuyer/Update/{buyerId}")]
        public async Task<IActionResult> Update(int discountId, string buyerId)
        {
            return await LoadUpdateModel(discountId, buyerId);
        }
        [HttpPost]
        [Route("Admin/Discount/{discountId:int}/DiscountBuyer/Update/{buyerId}")]
        public async Task<IActionResult> Update(UpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoadUpdateModel(updateModel.DiscountId, updateModel.BuyerId, updateModel);
            }

            var discountBuyer = await _onlineShopDbContext.DiscountBuyers
                .FirstOrDefaultAsync(p => p.DiscountId == updateModel.DiscountId);
            if (discountBuyer == null)
            {
                return RedirectToAction("Index", "Discount");
            }

            discountBuyer.IsUsed= updateModel.IsUsed;

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Update", "Discount",
                new { id = updateModel.DiscountId });
        }
        public async Task<IActionResult> LoadUpdateModel
            (int discountId,string buyerId, UpdateModel previousUpdateModel = null!)
        {
            var discountBuyer = await _onlineShopDbContext.DiscountBuyers
                .Include(t => t.Buyer)
                .Where(t => t.DiscountId == discountId)
                .Where(T => T.BuyerId == buyerId)
                .FirstOrDefaultAsync();

            if (discountBuyer == null)
            {
                return RedirectToAction("Index", "Discount");
            }

            if (previousUpdateModel != null)
            {
                var updateModel = new UpdateModel
                {
                    DiscountId = previousUpdateModel.DiscountId,
                    BuyerId = previousUpdateModel.BuyerId,
                    BuyerUserName = previousUpdateModel.BuyerUserName,
                    IsUsed = previousUpdateModel.IsUsed,
                };

                return View(updateModel);
            }

            return View(new UpdateModel
            {
                DiscountId = discountBuyer.DiscountId,
                BuyerId = discountBuyer.BuyerId,
                BuyerUserName = discountBuyer.Buyer.UserName!,
                IsUsed = discountBuyer.IsUsed ,
            });
        }

        [HttpGet]
        [Route("Admin/Discount/{discountId:int}/DiscountBuyer/Delete/{buyerId}")]
        public async Task<IActionResult> Delete(int discountId, string buyerId)
        {
            var discountBuyer = await _onlineShopDbContext.DiscountBuyers
                .Where(t => t.DiscountId == discountId)
                .Where(T => T.BuyerId == buyerId)
                .FirstOrDefaultAsync();

            if (discountBuyer == null)
            {
                return RedirectToAction("Index", "Discopunt");
            }

            _onlineShopDbContext.DiscountBuyers.Remove(discountBuyer);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Update", "Discount",
                new { id = discountId });
        }
    }
}