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

            var retrievedCarts = await queryableCarts.Skip(skip).Take(pageSize).
                Select(t =>
                new CartModel
                {
                    IdCart=t.IdCart,
                    BuyerUserName=t.Buyer.UserName!,
                    DiscountVoucher = t.Discount != null ? t.Discount.Voucher : string.Empty
                }).ToListAsync();
            recordsTotal = retrievedCarts.Count();

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
                IdCart=cart.IdCart,
                BuyerUserName = cart.Buyer.UserName!,
                DiscountVoucher = cart.Discount?.Voucher ?? string.Empty
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
