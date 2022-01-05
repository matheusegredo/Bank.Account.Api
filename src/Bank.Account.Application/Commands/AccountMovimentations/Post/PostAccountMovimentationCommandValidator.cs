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

            RuleFor(p => p.Value)
                .GreaterThanOrEqualTo(0)
                .When(p => p.ValueShouldBePositive);

            RuleFor(p => p.Value)
                .LessThan(0)
                .When(p => !p.ValueShouldBePositive);
        }
    }
}
