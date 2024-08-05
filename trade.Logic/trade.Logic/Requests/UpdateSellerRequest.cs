namespace trade.Logic.Requests
{
    public class UpdateSellerRequest
    {
        public Guid UserId { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
    }
}
