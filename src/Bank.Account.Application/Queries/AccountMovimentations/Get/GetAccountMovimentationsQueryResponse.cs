namespace Bank.Application.Queries.AccountMovimentations.Get
{
    public class GetAccountMovimentationsQueryResponse
    {
        public GetAccountMovimentationsQueryResponse(string accountNumber, decimal accountBalance)
        {
            Movimentations = new HashSet<AccountMovimentationsModel>();
            AccountNumber = accountNumber;
            AccountBalance = accountBalance;
        }

        public string AccountNumber { get; set; }

        public decimal AccountBalance { get; set; }

        public ICollection<AccountMovimentationsModel> Movimentations { get; set; }
    }

    public class AccountMovimentationsModel
    {
        public decimal Value { get; set; }

        public string Type { get; set; }

        public string Date { get; set; }
    }
}
