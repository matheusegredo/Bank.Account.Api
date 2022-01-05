namespace Bank.Application.Queries.AccountBalances
{
    public class GetAccountBalanceQuery : IRequest<GetAccountBalanceQueryResponse>
    {
        public GetAccountBalanceQuery(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; set; }
    }
}
