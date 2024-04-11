namespace Intex.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public int TransactionId { get; set; }
        public string CustomerName { get; set; }
        public DateOnly Date { get; set; }
        public string DayOfWeek { get; set; }
        public byte Hour { get; set; }
        public string EntryModeDescription { get; set; }
        public short Amount { get; set; }
        public string TransactionTypeDescription { get; set; }
        public string CountryOfTransaction { get; set; }
        public string ShippingAddress { get; set; }
        public string BankName { get; set; }
        public string CardTypeDescription { get; set; }
        public bool Fraud { get; set; }
    }

    public class OrdersViewModel
    {
        public IEnumerable<OrderDetailViewModel> Orders { get; set; }
    }
}

