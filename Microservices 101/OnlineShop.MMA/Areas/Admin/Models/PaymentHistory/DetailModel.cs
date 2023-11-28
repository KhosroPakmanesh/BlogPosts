namespace OnlineShop.MMA.Areas.Admin.Models.PaymentHistory
{
    public class DetailModel
    {
        public string BuyerUserName { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public DateTime PaymentDateTime { get; set; }
        public decimal PaymentValue { get; set; }
    }
}
