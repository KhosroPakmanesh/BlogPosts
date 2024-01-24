using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWebApplication.DbContexts;
using MVCWebApplication.Entities;
using MVCWebApplication.Models.OrderItem;
using QueryableExtensions.Extensions;

namespace MVCWebApplication.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly OrderDbContext _orderDbContext;

        public OrderItemController(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
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

            var queryableOrderItems = _orderDbContext.OrderItems
                .Include(t => t.Product)
                .Where(t => t.OrderId == parentId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                queryableOrderItems = queryableOrderItems
                    .Search(orderItem => new
                    {
                        orderItem.Product.Name,
                        orderItem.Quantity,
                        orderItem.Price,
                        orderItem.Product.Description
                    },
                    searchValue);
            }

            recordsTotal = await queryableOrderItems.CountAsync();
            var retrievedOrderItems = await queryableOrderItems
                .Skip(skip)
                .Take(pageSize)
                .Select(orderItem => new OrderItemModel
                {
                    IdOrderItem = orderItem.IdOrderItem,
                    OrderId = orderItem.OrderId,
                    ProductName = orderItem.Product.Name,
                    Quantity = orderItem.Quantity,
                    Price = orderItem.Price,
                    Description = orderItem.Product.Description
                })                
                .ToListAsync();

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = retrievedOrderItems
            };

            return Ok(responseObject);
        }

        [HttpGet]
        [Route("Order/{OrderId:int}/OrderItem/Detail/{idOrderItem:int}")]
        public async Task<IActionResult> Detail (int orderId, int idOrderItem)
        {
            var orderItem = await _orderDbContext.OrderItems
                .Include(t => t.Product)
                .Where(t => t.OrderId == orderId)
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
                Price = orderItem.Price,
                Description = orderItem.Product.Description
            });
        }
    }
}