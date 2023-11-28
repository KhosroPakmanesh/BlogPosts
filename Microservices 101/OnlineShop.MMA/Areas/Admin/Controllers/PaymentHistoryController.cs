using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Areas.Admin.Models.PaymentHistory;
using OnlineShop.MMA.Data.OnlineShopDbContext;


namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class PaymentHistoryController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public PaymentHistoryController(
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
        public async Task<IActionResult> GetPaymentHistories()
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

            var queryablePaymentHistories = _onlineShopDbContext.PaymentHistories
                .Include(t => t.Buyer)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryablePaymentHistories = queryablePaymentHistories.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    expectedCarts = expectedCarts.Where(
            //        m => m.Name.Contains(searchValue));
            //}

            recordsTotal = await queryablePaymentHistories.CountAsync();
            var retrievedPaymentHistories = await queryablePaymentHistories.Skip(skip).Take(pageSize).
                Select(t =>
                new PaymentHistoryModel
                {
                    IdPaymentHistory = t.IdPaymentHistory,
                    BuyerUserName = t.Buyer.UserName!,
                    BankAccountNumber=t.BankAccountNumber,
                    PaymentDateTime = t.PaymentDateTime,
                    PaymentValue = t.PaymentValue
                }).ToListAsync();

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = retrievedPaymentHistories
            };

            return Ok(responseObject);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var paymentHistory = await _onlineShopDbContext.PaymentHistories
                .Include(t => t.Buyer)
                .FirstOrDefaultAsync(t => t.IdPaymentHistory == id);

            if (paymentHistory == null)
            {
                return RedirectToAction("Index", "PaymentHistory");
            }

            return View(new DetailModel
            {
                BuyerUserName = paymentHistory.Buyer.UserName!,
                BankAccountNumber = paymentHistory.BankAccountNumber,
                PaymentDateTime = paymentHistory.PaymentDateTime,
                PaymentValue = paymentHistory.PaymentValue
            });
        }
    }
}
