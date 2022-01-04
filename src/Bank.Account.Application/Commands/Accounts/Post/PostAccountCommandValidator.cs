namespace Bank.Application.Commands.Accounts.Post
{
    public class PostAccountCommandValidator : AbstractValidator<PostAccountCommand>
    {
        public PostAccountCommandValidator()
        {
            RuleFor(p => p.ClientId)
                .NotEmpty();

            RuleFor(p => p.AccountNumber)
                .NotEmpty();
        }
    }
}
