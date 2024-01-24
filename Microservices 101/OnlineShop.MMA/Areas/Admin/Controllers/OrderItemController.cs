using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.OrderItem;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class OrderItemController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public OrderItemController(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderItems()
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

            var queryableOrderItems = _onlineShopDbContext.OrderItems
                .Include(t=>t.Product)
                .Where(t=>t.OrderId== parentId)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableOrderItems = queryableOrderItems.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    queryableShippings = queryableShippings.Where(
            //        m => m.Voucher.Contains(searchValue));
            //}

            recordsTotal = await queryableOrderItems.CountAsync();
            var rawOrderItems = await queryableOrderItems
                .Skip(skip).Take(pageSize)
                .ToListAsync();

            var formattedOrderItems = new List<OrderItemModel>();
            foreach (var rawOrderItem in rawOrderItems)
            {
                formattedOrderItems.Add(new OrderItemModel
                {
                    IdOrderItem = rawOrderItem.IdOrderItem,
                    OrderId=rawOrderItem.OrderId,
                    ProductName = rawOrderItem?.Product?.Name!,
                    Quantity= rawOrderItem?.Quantity ?? 0,
                    Price= rawOrderItem?.Price ?? 0
                });
            }

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = formattedOrderItems
            };

            return Ok(responseObject);
        }

        [HttpGet]
        [Route("Admin/Order/{OrderId:int}/OrderItem/Detail/{idOrderItem:int}")]
        public async Task<IActionResult> Detail 
            (int orderId, int idOrderItem)
        {
            var orderItem = await _onlineShopDbContext.OrderItems
                .Include(t => t.Product)
                .Where(t=>t.OrderId== orderId)
                .Where(t => t.IdOrderItem == idOrderItem)
                .FirstOrDefaultAsync();

            if (orderItem == null)
            {
                return RedirectToAction("Index", "Order");
            }

            return View(new DetailModel
            {
                IdOrderItem = orderItem.IdOrderItem,
                OrderId = orderItem.OrderId,
                ProductName = orderItem.Product.Name,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price
            });
        }
    }
}