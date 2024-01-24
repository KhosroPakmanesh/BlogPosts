using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWebApplication.DbContexts;
using MVCWebApplication.Models.Home;
using QueryableExtensions.Extensions;

namespace MVCWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly OrderDbContext _orderDbContext;

        public HomeController(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
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

            var queryableOrders = _orderDbContext.Orders
                .Include(t => t.User)
                .Include(t => t.Payment)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                queryableOrders = queryableOrders
                    .Search(order => new
                    {
                        order.User.UserName,
                        OrderDateTime = order.DateTime,
                        order.Status,
                        order.Payment.Value,
                        PaymentDateTime = order.Payment.DateTime
                    },
                    searchValue);

                //var enumerableOrders = DataProvider.GetEnumerableOrders();
                //var retrievedEnumerableOrders = enumerableOrders
                //    .Search(order => new
                //    {
                //        order.User.UserName,
                //        OrderDateTime = order.DateTime,
                //        order.Status,
                //        order.Payment.Value,
                //        PaymentDateTime = order.Payment.DateTime
                //    }, searchValue)
                //    .ToList();
            }

            try
            {
                recordsTotal = queryableOrders.Count();
                retrievedOrders = await queryableOrders
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(order =>
                        new OrderModel
                        {
                            IdOrder = order.IdOrder,
                            UserUserName = order.User.UserName!,
                            OrderDateTime = order.DateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                            OrderStatus = order.Status,
                            PaymentDateTime = order.Payment.DateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                            PaymentValue = order.Payment.Value
                        })
                    .ToListAsync();

            }
            catch { }

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
            var order = await _orderDbContext.Orders
                .Include(t => t.User)
                .Include(t => t.Payment)
                .FirstOrDefaultAsync(t => t.IdOrder == id);

            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new DetailModel
            {
                IdOrder = order.IdOrder,
                UserUserName = order.User.UserName,
                UserFirstName = order.User.FirstName,
                UserLastName = order.User.LastName,
                OrderDateTime = order.DateTime,
                OrderStatus = order.Status,
                BankAccountNumber = order.Payment.BankAccountNumber,
                PaymentDateTime = order.Payment.DateTime,
                PaymentValue = order.Payment.Value
            });
        }
    }
}
