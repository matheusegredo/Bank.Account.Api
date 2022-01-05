namespace Bank.Application.Queries.AccountMovimentations.Get
{
    public class GetAccountMovimentationsQueryValidator : AbstractValidator<GetAccountMovimentationsQuery>
    {
        public GetAccountMovimentationsQueryValidator()
        {
            RuleFor(p => p.AccountId)
                .NotEmpty();

            RuleFor(p => p.InitialDate)
                .NotEmpty()
                .When(p => p.FinalDate.HasValue);

            RuleFor(p => p.FinalDate)
                .NotEmpty()
                .When(p => p.InitialDate.HasValue);

            RuleFor(p => p.FinalDate)
                .GreaterThanOrEqualTo(p => p.InitialDate)
                .When(p => p.FinalDate.HasValue);
        }
    }
}
