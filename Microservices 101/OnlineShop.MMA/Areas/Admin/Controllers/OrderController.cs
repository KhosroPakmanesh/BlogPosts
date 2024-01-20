using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.Order;
using OnlineShop.MMA.Areas.Admin.Controllers.Extensions;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class OrderController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public OrderController(
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
        public async Task<IActionResult> GetOrders()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            List<OrderModel> retrievedOrders = new();
            int recordsTotal = 0;

            var queryableOrders = _onlineShopDbContext.Orders
                .Include(t => t.Buyer)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && 
                string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableOrders = queryableOrders
                    .OrderBy(sortColumn + " " + sortColumnDirection);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                queryableOrders = queryableOrders
                    .Search(order => new
                    { 
                        order.Buyer.UserName,
                        OrderDateTime= order.DateTime,
                        order.Status,
                        order.Payment.Value,
                        PaymentDateTime=order.Payment.DateTime
                    },
                    searchValue);            
            }

            try
            {
                retrievedOrders = await queryableOrders
                .Include(t => t.Buyer)
                .Include(t => t.Payment)
                .Skip(skip)
                .Take(pageSize)
                .Select(order =>
                    new OrderModel
                    {
                        IdOrder = order.IdOrder,
                        BuyerUserName = order.Buyer.UserName!,
                        OrderDateTime = order.DateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                        OrderStatus = order.Status,
                        PaymentDateTime = order.Payment.DateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                        PaymentValue = order.Payment.Value
                    })
                .ToListAsync();
                recordsTotal = retrievedOrders.Count();
            }
            catch{}

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = retrievedOrders
            };

            return Ok(responseObject);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var order = await _onlineShopDbContext.Orders
                .Include(t => t.Buyer)
                .Include(t=>t.Payment)
                .FirstOrDefaultAsync(t => t.IdOrder == id);

            if (order == null)
            {
                return RedirectToAction("Index", "Order");
            }

            return View(new DetailModel
            {
                IdOrder=order.IdOrder,
                BuyerUserName = order.Buyer.UserName!,
                OrderDateTime = order.DateTime,
                OrderStatus = order.Status,
                BankAccountNumber= order.Payment.BankAccountNumber,
                PaymentDateTime = order.Payment.DateTime,
                PaymentValue = order.Payment.Value
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _onlineShopDbContext.Orders
                .Include(t => t.Buyer)
                .FirstOrDefaultAsync(t => t.IdOrder == id);

            if (order == null)
            {
                return RedirectToAction("Index", "Order");
            }

            _onlineShopDbContext.Orders.Remove(order);
            await _onlineShopDbContext.SaveChangesAsync(); ;

            return RedirectToAction("Index", "Order");
        }
    }
}
