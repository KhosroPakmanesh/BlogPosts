namespace OnlineShop.MMA.Areas.Admin.Models.CartItem
{
    public class CartItemModel
    {
        public int IdCartItem { get; set; }
        public int CartId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
