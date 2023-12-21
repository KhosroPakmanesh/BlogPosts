namespace OnlineShop.MMA.Areas.Admin.Models.Order
{
    public class DetailModel
    {
        public int IdOrder { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;
        public DateTime OrderDateTime { get; set; }
        public byte OrderStatus { get; set; }

        public string BankAccountNumber { get; set; } = null!;
        public DateTime PaymentDateTime { get; set; }
        public decimal PaymentValue { get; set; }
    }
}
