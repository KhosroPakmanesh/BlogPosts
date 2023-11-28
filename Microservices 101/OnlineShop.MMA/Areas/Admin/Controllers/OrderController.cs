using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.Order;

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
            int recordsTotal = 0;

            var queryableOrders = _onlineShopDbContext.Orders
                .Include(t=>t.Buyer)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableOrders = queryableOrders.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    queryableOrders = queryableOrders.Where(
            //        m => m.Name.Contains(searchValue));
            //}

            recordsTotal = await queryableOrders.CountAsync();
            var retrievedProductTypes = await queryableOrders.Skip(skip).Take(pageSize).
                Select(t =>
                new OrderModel
                {
                    IdOrder=t.IdOrder,
                    BuyerUserName=t.Buyer.UserName!,
                    OrderDateTime=t.OrderDateTime,
                    OrderStatus=t.OrderStatus
                }).ToListAsync();

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = retrievedProductTypes
            };

            return Ok(responseObject);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var order = await _onlineShopDbContext.Orders
                .Include(t => t.Buyer)
                .FirstOrDefaultAsync(t => t.IdOrder == id);

            if (order == null)
            {
                return RedirectToAction("Index", "Order");
            }

            return View(new DetailModel
            {
                BuyerUserName = order.Buyer.UserName!,
                OrderDateTime = order.OrderDateTime,
                OrderStatus = order.OrderStatus
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
                return await LoadUpdateModel(updateModel.IdOrder, updateModel);
            }

            var order = await _onlineShopDbContext.Orders
                .FirstOrDefaultAsync(t => t.IdOrder == updateModel.IdOrder);

            if (order == null)
            {
                return RedirectToAction("Index", "Order");
            }

            order.OrderStatus = updateModel.OrderStatus;
            _onlineShopDbContext.Orders.Update(order);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Order");
        }
        public async Task<IActionResult> 
            LoadUpdateModel(int orderId, UpdateModel previousUpdateModel= null)
        {
            var order =await _onlineShopDbContext.Orders
                .Include(t=>t.Buyer.UserName)
                .FirstOrDefaultAsync(t => t.IdOrder == orderId);

            if (order == null)
            {
                return RedirectToAction("Index", "Order");
            }

            if (previousUpdateModel != null)
            {
                return View(new UpdateModel
                {
                    IdOrder = previousUpdateModel.IdOrder,
                    BuyerUserName = previousUpdateModel.BuyerUserName,
                    OrderStatus = previousUpdateModel.OrderStatus
                });
            }

            return View(new UpdateModel
            {
                IdOrder = orderId,
                BuyerUserName = order.Buyer.UserName!,
                OrderStatus = order.OrderStatus
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
