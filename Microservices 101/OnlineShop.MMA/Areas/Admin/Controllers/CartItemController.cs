using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.CartItem;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CartItemController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public CartItemController(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCartItems()
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

            var queryableCartItems = _onlineShopDbContext.CartItems
                .Include(t=>t.Product)
                .Where(t=>t.CartId== parentId)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableCartItems = queryableCartItems.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    queryableShippings = queryableShippings.Where(
            //        m => m.Voucher.Contains(searchValue));
            //}

            recordsTotal = await queryableCartItems.CountAsync();
            var rawCartItems = await queryableCartItems
                .Skip(skip).Take(pageSize)
                .ToListAsync();            

            var formattedCartItems = new List<CartItemModel>();
            foreach (var rawCartItem in rawCartItems)
            {
                formattedCartItems.Add(new CartItemModel
                {
                    IdCartItem = rawCartItem.IdCartItem,
                    CartId=rawCartItem.CartId,
                    ProductName = rawCartItem?.Product?.Name!,
                    Quantity= rawCartItem?.Quantity ?? 0,
                    Price= rawCartItem?.Price ?? 0
                });
            }

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = formattedCartItems
            };

            return Ok(responseObject);
        }

        [HttpGet]
        [Route("Admin/Cart/{CartId:int}/CartItem/Detail/{idCartItem:int}")]
        public async Task<IActionResult> Detail 
            (int cartId, int idCartItem)
        {
            var cartItem = await _onlineShopDbContext.CartItems
                .Include(t => t.Product)
                .Where(t=>t.CartId== cartId)
                .Where(t => t.IdCartItem == idCartItem)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            return View(new DetailModel
            {
                IdCartItem = cartItem.IdCartItem,
                CartId = cartItem.CartId,
                ProductName = cartItem.Product.Name,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            });
        }
    }
}