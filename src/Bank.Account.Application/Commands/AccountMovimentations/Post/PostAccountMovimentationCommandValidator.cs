namespace Bank.Application.Commands.AccountMovimentations.Post
{
    public class PostAccountMovimentationCommandValidator : AbstractValidator<PostAccountMovimentationCommand>
    {
        public PostAccountMovimentationCommandValidator()
        {
            RuleFor(p => p.AccountId)
                .NotEmpty();
            
            RuleFor(p => p.Type)
                .IsInEnum();
        }
    }
}
