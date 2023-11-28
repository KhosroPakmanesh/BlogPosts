namespace OnlineShop.MMA.Areas.Admin.Models.Product
{
    public class DetailModel
    {
        public string ProductTypeName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
