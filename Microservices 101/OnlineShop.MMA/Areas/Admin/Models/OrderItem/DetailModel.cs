﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop.MMA.Areas.Admin.Models.OrderItem
{
    public class DetailModel
    {
        public int IdOrderItem { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
