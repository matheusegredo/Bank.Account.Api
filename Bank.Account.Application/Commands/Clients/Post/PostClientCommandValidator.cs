global using FluentValidation;

namespace Bank.Application.Commands.Clients.Post
{
    public class PostClientCommandValidator : AbstractValidator<PostClientCommand>
    {
        public PostClientCommandValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty();

            RuleFor(p => p.LastName)
                .NotEmpty();

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
