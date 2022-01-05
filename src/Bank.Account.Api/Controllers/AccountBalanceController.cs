using Bank.Api.Infrastructure.Base;
using Bank.Application.Commands.Accounts.Post;
using Bank.Application.Queries.AccountBalances;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountBalanceController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AccountBalanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> Get([FromRoute] int accountId) => await ResponseHandler(() => _mediator.Send(new GetAccountBalanceQuery(accountId)));
    }
}
