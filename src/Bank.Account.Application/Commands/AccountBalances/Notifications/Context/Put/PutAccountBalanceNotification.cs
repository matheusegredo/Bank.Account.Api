namespace Bank.Application.Commands.AccountBalances.Notifications
{
    public class PutAccountBalanceNotification : INotification
    {
        public PutAccountBalanceNotification(int accountId, decimal value)
        {
            AccountId = accountId;
            Value = value;
        }

        public int AccountId { get; set; }

        public decimal Value { get; set; }

        public string CacheKey => $"AccountId:{AccountId}";
    }
}
