namespace MVCWebApplication.Models.Home
{
    public class DetailModel
    {
        public int IdOrder { get; set; }
        public string UserUserName { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public DateTime OrderDateTime { get; set; }
        public int OrderStatus { get; set; }

        public string BankAccountNumber { get; set; } = null!;
        public DateTime PaymentDateTime { get; set; }
        public decimal PaymentValue { get; set; }
    }
}
