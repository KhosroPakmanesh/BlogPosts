using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Areas.Admin.Models.Cart;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CartController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public CartController(
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
        public async Task<IActionResult> GetCarts()
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

            var queryableCarts = _onlineShopDbContext.Carts
                .Include(t=>t.Buyer)
                .Include(t => t.Discount)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableCarts = queryableCarts.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    expectedCarts = expectedCarts.Where(
            //        m => m.Name.Contains(searchValue));
            //}

            recordsTotal = await queryableCarts.CountAsync();
            var retrievedCarts = await queryableCarts.Skip(skip).Take(pageSize).
                Select(t =>
                new CartModel
                {
                    IdCart=t.IdCart,
                    BuyerUserName=t.Buyer.UserName!,
                    DiscountVoucher = t.Discount.Voucher
                }).ToListAsync();

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = retrievedCarts
            };

            return Ok(responseObject);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var cart = await _onlineShopDbContext.Carts
                .Include(t=>t.Buyer)
                .FirstOrDefaultAsync(t => t.IdCart == id);

            if (cart == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            return View(new DetailModel
            {
                BuyerUserName = cart.Buyer.UserName!,
                DiscountVoucher = cart.Discount.Voucher
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
                return await LoadUpdateModel(updateModel.IdCart, updateModel);
            }

            var cart = await _onlineShopDbContext.Carts
                .FirstOrDefaultAsync(t => t.IdCart == updateModel.IdCart);

            if (cart == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            cart.DiscountId = updateModel.DiscountId;
            _onlineShopDbContext.Carts.Update(cart);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }
        public async Task<IActionResult> 
            LoadUpdateModel(int cartId, UpdateModel previousUpdateModel= null)
        {
            var cart =await _onlineShopDbContext.Carts
                .Include(t=>t.Buyer)
                .FirstOrDefaultAsync(t => t.IdCart == cartId);

            if (cart == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            var buyers = await _onlineShopDbContext.AspNetUsers.ToListAsync();

            var discounts = await _onlineShopDbContext.Discounts.ToListAsync();
            var discountTypesSelectListItems = new List<SelectListItem>();
            foreach (var discount in discounts)
            {
                discountTypesSelectListItems.Add(new SelectListItem
                {
                    Value = discount.IdDiscount.ToString(),
                    Text = discount?.Voucher
                });
            }

            if (previousUpdateModel != null)
            {
                return View(new UpdateModel
                {
                    IdCart = previousUpdateModel.IdCart,
                    BuyerUserName = previousUpdateModel.BuyerUserName,
                    DiscountId = previousUpdateModel.DiscountId
                });
            }

            return View(new UpdateModel
            {
                IdCart = cartId,
                BuyerUserName = cart.Buyer.UserName!,
                DiscountId = cart.DiscountId!,
                DiscountSelectListItems=discountTypesSelectListItems
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _onlineShopDbContext.Carts
                .FirstOrDefaultAsync(t => t.IdCart == id);

            if (cart == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            _onlineShopDbContext.Carts.Remove(cart);
            await _onlineShopDbContext.SaveChangesAsync(); ;

            return RedirectToAction("Index", "Cart");
        }
    }
}
