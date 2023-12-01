using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.ShippingLeg;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ShippingLegController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public ShippingLegController(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetShippingLegs()
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

            var queryableShippingLegs = _onlineShopDbContext.ShippingLegs
                .Where(t=>t.ShippingId== parentId)
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableShippingLegs = queryableShippingLegs.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    queryableShippings = queryableShippings.Where(
            //        m => m.Voucher.Contains(searchValue));
            //}

            var rawShippingLegs = await queryableShippingLegs
                .Skip(skip).Take(pageSize)
                .ToListAsync();
            recordsTotal = rawShippingLegs.Count();

            var formattedShippingLegs = new List<ShippingLegModel>();
            foreach (var rawShippingLeg in rawShippingLegs)
            {
                formattedShippingLegs.Add(new ShippingLegModel
                {
                    IdShippingLeg = rawShippingLeg.IdShippingLeg,
                    ShippingId=rawShippingLeg.ShippingId,
                    Address = rawShippingLeg?.Address!,
                    IsShipped = rawShippingLeg?.IsShipped ?? false
                });
            }

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = formattedShippingLegs
            };

            return Ok(responseObject);
        }

        [HttpGet]
        [Route("Admin/Shipping/{shippingId:int}/ShippingLeg/Detail/{idshippingLeg:int}")]
        public async Task<IActionResult> Detail 
            (int shippingId, int idshippingLeg,
            [FromQuery(Name = "previousPage")] string previousPage)
        {
            var shippingLeg = await _onlineShopDbContext.ShippingLegs
                .FirstOrDefaultAsync(p => p.IdShippingLeg == idshippingLeg);

            if (shippingLeg == null)
            {
                return RedirectToAction("Index", "Shipping", new { id = shippingId});
            }

            return View(new DetailModel
            {
                ShippingId = shippingLeg.ShippingId,
                Address = shippingLeg?.Address!,
                IsShipped = shippingLeg?.IsShipped ?? false,
                PreviousAction= previousPage 
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            return await LoadCreateModel(id);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoadCreateModel(createModel.ShippingId, createModel);
            }

            await _onlineShopDbContext.ShippingLegs
                .AddAsync(new ShippingLeg
            {
                ShippingId=createModel.ShippingId,
                Address = createModel.Address,
                IsShipped = createModel.IsShipped,
            });

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Update", "Shipping",
                new { id = createModel.ShippingId });
        }
        public async Task<IActionResult> LoadCreateModel
            (int id, CreateModel previousCreateModel = null!)
        {
            var shipping = await _onlineShopDbContext.Shippings
                .FirstOrDefaultAsync(p => p.IdShipping == id);

            if (shipping == null)
            {
                return RedirectToAction("Index", "Shipping");
            }

            var createModel = new CreateModel
            {
                ShippingId = id,
                Address = previousCreateModel?.Address!,
                IsShipped = previousCreateModel?.IsShipped ?? false,
            };

            return View(createModel);
        }

        [HttpGet]
        [Route("Admin/Shipping/{shippingId:int}/ShippingLeg/Update/{idshippingLeg:int}")]
        public async Task<IActionResult> Update(int shippingId, int idshippingLeg)
        {
            return await LoadUpdateModel(idshippingLeg);
        }
        [HttpPost]
        [Route("Admin/Shipping/{shippingId:int}/ShippingLeg/Update/{idshippingLeg:int}")]
        public async Task<IActionResult> Update(UpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoadUpdateModel(updateModel.IdShippingLeg, updateModel);
            }

            var shippingLeg = await _onlineShopDbContext.ShippingLegs
                .FirstOrDefaultAsync(p => p.IdShippingLeg == updateModel.IdShippingLeg);
            if (shippingLeg == null)
            {
                return RedirectToAction("Index", "Shipping");
            }

            shippingLeg.Address = updateModel.Address;
            shippingLeg.IsShipped = updateModel.IsShipped;

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Update", "Shipping",
                new { id = updateModel.ShippingId });
        }
        public async Task<IActionResult> LoadUpdateModel
            (int id, UpdateModel previousUpdateModel = null!)
        {
            var shippingLeg = await _onlineShopDbContext.ShippingLegs
                .FirstOrDefaultAsync(p => p.IdShippingLeg == id);

            if (shippingLeg == null)
            {
                return RedirectToAction("Index", "Shipping");
            }

            if (previousUpdateModel != null)
            {
                var updateModel = new UpdateModel
                {
                    IdShippingLeg = previousUpdateModel.IdShippingLeg,
                    ShippingId = previousUpdateModel.ShippingId,
                    Address = previousUpdateModel.Address,
                    IsShipped = previousUpdateModel.IsShipped,
                };

                return View(updateModel);
            }

            return View(new UpdateModel
            {
                IdShippingLeg = shippingLeg.IdShippingLeg,
                ShippingId = shippingLeg.ShippingId,
                Address = shippingLeg.Address!,
                IsShipped = shippingLeg.IsShipped,
            });
        }

        [HttpGet]
        [Route("Admin/Shipping/{shippingId:int}/ShippingLeg/Delete/{idshippingLeg:int}")]
        public async Task<IActionResult> Delete(int shippingId, int idshippingLeg)
        {
            var shippingLeg = await _onlineShopDbContext.ShippingLegs
                .FirstOrDefaultAsync(t => t.IdShippingLeg == idshippingLeg);

            if (shippingLeg == null)
            {
                return RedirectToAction("Update", "Shipping",
                    new { id = shippingId });
            }

            _onlineShopDbContext.ShippingLegs.Remove(shippingLeg);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Update", "Shipping",
                new { id = shippingId });
        }
    }
}