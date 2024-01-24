﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using OnlineShop.MMA.Areas.Admin.Models.Discount;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class DiscountController : Controller
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public DiscountController(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetDiscounts()
        {
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

            var queryableDiscounts = _onlineShopDbContext.Discounts
                .AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                queryableDiscounts = queryableDiscounts.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    queryableDiscounts = queryableDiscounts.Where(
            //        m => m.Voucher.Contains(searchValue));
            //}

            recordsTotal = await queryableDiscounts.CountAsync();
            var rawDiscounts = await queryableDiscounts
                .Skip(skip).Take(pageSize)
                .ToListAsync();

            var formattedDiscounts = new List<DiscountModel>();
            foreach (var rawDiscount in rawDiscounts)
            {
                formattedDiscounts.Add(new DiscountModel
                {
                    IdDiscount= rawDiscount.IdDiscount,
                    Voucher=rawDiscount.Voucher,
                    ReductionPercentage=rawDiscount.ReductionPercentage,
                });
            }

            var responseObject = new
            {
                draw,
                recordsFiltered = recordsTotal,
                recordsTotal,
                data = formattedDiscounts
            };

            return Ok(responseObject);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var discount = await _onlineShopDbContext.Discounts
                .Include(t => t.DiscountBuyers)
                    .ThenInclude(t => t.Buyer)
                .FirstOrDefaultAsync(p => p.IdDiscount == id);

            if (discount == null)
            {
                return RedirectToAction("Index", "Discount");
            }

            //var buyers = await _onlineShopDbContext.AspNetUsers.ToListAsync();
            //var buyerSelectListItems = new List<SelectListItem>();
            //foreach (var buyer in buyers)
            //{
            //    buyerSelectListItems.Add(new SelectListItem
            //    {
            //        Value = buyer?.Id,
            //        Text = buyer?.UserName
            //    });
            //}

            return View(new DetailModel
            {
                IdDiscount = id,
                Voucher = discount.Voucher,
                ReductionPercentage = discount.ReductionPercentage,
                //BuyerIds = discount.DiscountBuyers.Select(t => t.BuyerId).ToList(),
                //BuyerSelectListItems = buyerSelectListItems,
            });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return await LoadCreateModel();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoadCreateModel(createModel);
            }

            //var discountBuyers = new List<DiscountBuyer>();
            //foreach (var buyerId in createModel.BuyerIds)
            //{
            //    discountBuyers.Add(new DiscountBuyer
            //    {
            //        BuyerId = buyerId,
            //        IsUsed = false
            //    });
            //}

            _onlineShopDbContext.Discounts.Add(new Discount
            {
                Voucher = createModel.Voucher,
                ReductionPercentage = createModel.ReductionPercentage,
                //DiscountBuyers = discountBuyers,
            });

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Discount");
        }
        public async Task<IActionResult> LoadCreateModel
            (CreateModel previousCreateModel = null!)
            {
                //var buyers = await _onlineShopDbContext.AspNetUsers.ToListAsync();
                //var buyerSelectListItems = new List<SelectListItem>();
                //foreach (var buyer in buyers)
                //{
                //    buyerSelectListItems.Add(new SelectListItem
                //    {
                //        Value = buyer.Id,
                //        Text = buyer.UserName
                //    });
                //}

                var createModel = new CreateModel
                {
                    Voucher = previousCreateModel?.Voucher!,
                    ReductionPercentage = previousCreateModel?.ReductionPercentage ?? 0,
                    //BuyerSelectListItems = buyerSelectListItems
                };

                return View(createModel);
            }
        
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            return await LoadUpdateModel(id);
        }
        public async Task<IActionResult> LoadUpdateModel
            (int id, UpdateModel previousUpdateModel = null!)
        {
            var discount = await _onlineShopDbContext.Discounts
                .Include(t => t.DiscountBuyers)
                    .ThenInclude(t => t.Buyer)
                .FirstOrDefaultAsync(p => p.IdDiscount == id);

            if (discount == null)
            {
                return RedirectToAction("Index", "Discount");
            }

            //var buyers = await _onlineShopDbContext.AspNetUsers.ToListAsync();
            //var buyerSelectListItems = new List<SelectListItem>();
            //foreach (var buyer in buyers)
            //{
            //    buyerSelectListItems.Add(new SelectListItem
            //    {
            //        Value = buyer.Id,
            //        Text = buyer.UserName
            //    });
            //}

            if (previousUpdateModel != null)
            {

                var updateModel = new UpdateModel
                {
                    IdDiscount = previousUpdateModel.IdDiscount,
                    Voucher = previousUpdateModel?.Voucher!,
                    ReductionPercentage = previousUpdateModel?.ReductionPercentage ?? 0,
                    //BuyerIds = previousUpdateModel?.BuyerIds!,
                    //BuyerSelectListItems = buyerSelectListItems,
                };

                return View(updateModel);
            }

            return View(new UpdateModel
            {
                IdDiscount = discount.IdDiscount,
                Voucher = discount?.Voucher!,
                ReductionPercentage = discount?.ReductionPercentage ?? 0,
                //BuyerIds = discount?.DiscountBuyers.Select(t => t.BuyerId).ToList()!,
                //BuyerSelectListItems = buyerSelectListItems
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateModel updateModel)
        {
            if (!ModelState.IsValid)
            {
                return await LoadUpdateModel(updateModel.IdDiscount, updateModel);
            }

            var discount = await _onlineShopDbContext.Discounts
                .Include(t => t.DiscountBuyers)
                    .ThenInclude(t => t.Buyer)
                .FirstOrDefaultAsync(p => p.IdDiscount == updateModel.IdDiscount);
            if (discount == null)
            {
                return RedirectToAction("Index", "Discount");
            }

            //var removedBuyerIds = discount.DiscountBuyers
            //    .Select(pc => pc.BuyerId)
            //    .Except(updateModel.BuyerIds)
            //    .ToList();
            //var addedBuyerIds = updateModel.BuyerIds
            //    .Except(discount.DiscountBuyers
            //    .Select(pc => pc.BuyerId))
            //    .ToList();

            //var discountBuyers = new List<DiscountBuyer>();
            //if (removedBuyerIds.Any() || addedBuyerIds.Any())
            //{
            //    foreach (var removedBuyerId in removedBuyerIds)
            //    {
            //        var removedDiscountBuyer = discount.DiscountBuyers
            //            .FirstOrDefault(t => t.BuyerId == removedBuyerId);
            //        if (removedDiscountBuyer != null)
            //        {
            //            discount.DiscountBuyers.Remove(removedDiscountBuyer);
            //        }
            //    }

            //    foreach (var buyerId in updateModel.BuyerIds)
            //    {
            //        discountBuyers.Add(new DiscountBuyer
            //        {
            //            BuyerId = buyerId,
            //            DiscountId = discount.IdDiscount,
            //            IsUsed = false
            //        });
            //    }
            //}

            discount.Voucher = updateModel.Voucher;
            discount.ReductionPercentage = updateModel.ReductionPercentage;
            //discount.DiscountBuyers = discountBuyers;

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Discount");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var discount = await _onlineShopDbContext.Discounts
                .Include(t=>t.Carts)
                .Include(t=>t.DiscountBuyers)
                .FirstOrDefaultAsync(t=>t.IdDiscount==id);

            if (discount == null)
            {
                return RedirectToAction("Index", "Discount");
            }

            if (discount.Carts.Any())
            {
                ModelState.AddModelError("Error", "There are some carts associated with this discount. " +
                    "You can delete a discount when there is no carts associated with it.");
                return RedirectToAction("Index", "Discount");
            }

            _onlineShopDbContext.DiscountBuyers.RemoveRange(discount.DiscountBuyers);
            _onlineShopDbContext.Discounts.Remove(discount);

            await _onlineShopDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Discount");
        }
    }
}