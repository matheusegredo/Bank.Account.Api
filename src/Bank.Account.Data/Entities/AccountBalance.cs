namespace Bank.Data.Entities
{
    public class AccountBalance
    {
        public int AccountBalanceId { get; set; }

        public int AccountId { get; set; }

        public decimal Value { get; set; }

        public DateTime LastTimeChanged { get; set; }

        public Account? Account { get; set; }
    }
}
