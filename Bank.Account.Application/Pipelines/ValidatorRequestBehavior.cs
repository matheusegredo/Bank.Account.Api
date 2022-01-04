namespace Bank.Application.Pipelines
{
    public class ValidatorRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidatorRequestBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var errors = _validator.Validate(request).Errors;

            if (errors.Any())
                throw new ValidationException(errors);

            return next();
        }
    }
}
