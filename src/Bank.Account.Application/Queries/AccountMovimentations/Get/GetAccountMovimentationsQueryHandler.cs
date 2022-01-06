using Bank.Application.Helpers;
using Bank.Application.Queries.AccountBalances;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;
using Bank.Infrastructure.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Queries.AccountMovimentations.Get
{
    public class GetAccountMovimentationsQueryHandler : IRequestHandler<GetAccountMovimentationsQuery, GetAccountMovimentationsQueryResponse>
    {   
        private readonly IMediator _mediator;
        private readonly IBankContext _bankContext;
        private readonly IAuthenticationHelper _authenticationHelper;

        public GetAccountMovimentationsQueryHandler(IMediator mediator, IBankContext bankContext, IAuthenticationHelper authenticationHelper)
        {
            _mediator = mediator;
            _bankContext = bankContext;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<GetAccountMovimentationsQueryResponse> Handle(GetAccountMovimentationsQuery query, CancellationToken cancellationToken)
        {
            var account = await _bankContext.Accounts
               .Where(account => account.AccountId == query.AccountId)
               .FirstOrDefaultReadingUncomittedAsync(cancellationToken);

            if (account is null)
                throw new NotFoundException("Account not found.");

            _authenticationHelper.ValidateTokenByAccount(account, cancellationToken);

            var accountBalance = await _mediator.Send(new GetAccountBalanceQuery(query.AccountId), cancellationToken);

            var response = new GetAccountMovimentationsQueryResponse(account.AccountNumber, accountBalance.Value);

            var movimentationsQuery = _bankContext.AccountMovimentations.AsQueryable();
            movimentationsQuery = movimentationsQuery.Where(accountMovimentation => accountMovimentation.AccountId == query.AccountId);

            if (query.FilterByDate())
                movimentationsQuery = movimentationsQuery.Where(accountMovimentation => 
                    accountMovimentation.CreatedOn.Date >= query.InitialDate.GetValueOrDefault().Date && 
                    accountMovimentation.CreatedOn.Date <= query.FinalDate.GetValueOrDefault().Date);;

            var reponseMovimentationQuery = await movimentationsQuery.Select(accountMovimentation => new AccountMovimentationsModel
            {
                Value = accountMovimentation.Value,
                Type = accountMovimentation.Type.ToString(),
                Date = accountMovimentation.CreatedOn.ToShortDateString()
            }).ToListAsync(cancellationToken);

            response.Movimentations = reponseMovimentationQuery.OrderByDescending(p => p.Date).ToList();
            return response;
        }
    }
}
