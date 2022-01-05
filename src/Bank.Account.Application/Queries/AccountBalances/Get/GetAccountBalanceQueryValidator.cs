namespace Bank.Application.Queries.AccountBalances
{
    public class GetAccountBalanceQueryValidator : AbstractValidator<GetAccountBalanceQuery>
    {
        public GetAccountBalanceQueryValidator()
        {
            RuleFor(p => p.AccountId)
                .NotEmpty();
        }
    }
}
