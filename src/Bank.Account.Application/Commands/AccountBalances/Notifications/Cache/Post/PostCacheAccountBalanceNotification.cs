namespace Bank.Application.Commands.AccountBalances.Notifications.Post
{
    public class PostCacheAccountBalanceNotification : INotification
    {
        public PostCacheAccountBalanceNotification(int accountId, decimal value)
        {
            AccountId = accountId;
            Value = value;
        }

        public int AccountId { get; set; }

        public decimal Value { get; set; }

        public string CacheKey => $"AccountId:{AccountId}";
    }
}
