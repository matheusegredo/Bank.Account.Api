namespace Bank.Application.Queries.AccountMovimentations.Get
{
    public class GetAccountMovimentationsQuery : IRequest<GetAccountMovimentationsQueryResponse>
    {
        public int AccountId { get; set; }

        public DateTime? InitialDate { get; set; }

        public DateTime? FinalDate { get; set; }

        public bool FilterByDate() => InitialDate != null && FinalDate != null;
    }
}
