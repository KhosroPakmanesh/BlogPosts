using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Areas.Admin.Models.OrderHistory;
using OnlineShop.MMA.Data.OnlineShopDbContext;


namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class OrderHistoryController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public OrderHistoryController(
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
        public async Task<IActionResult> GetOrderHistories()
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

            var queryableOrderHistories = _onlineShopDbContext.OrderHistory
                .Include(t => t.Order)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableOrderHistories = queryableOrderHistories.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    expectedCarts = expectedCarts.Where(
            //        m => m.Name.Contains(searchValue));
            //}

            recordsTotal = await queryableOrderHistories.CountAsync();
            var retrievedOrderHistories = await queryableOrderHistories.Skip(skip).Take(pageSize).
                Select(t =>
                new OrderHistoryModel
                {
                    IdOrderHistory = t.IdOrderHistory,
                    OrderStatus = t.OrderStatus,
                }).ToListAsync();

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = retrievedOrderHistories
            };

            return Ok(responseObject);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var orderHistory = await _onlineShopDbContext.OrderHistory
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.IdOrderHistory == id);

            if (orderHistory == null)
            {
                return RedirectToAction("Index", "OrderHistory");
            }

            return View(new DetailModel
            {
                OrderStatus = orderHistory.OrderStatus,
            });
        }
    }
}
