using Bank.Api.Infrastructure.Base;
using Bank.Application.Commands.AccountMovimentations.Post;
using Bank.Application.Queries.AccountMovimentations.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("api/account/movimentation")]
    public class AccountMovimentationController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AccountMovimentationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostAccountMovimentationCommand command) => await ResponseHandler(() => _mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAccountMovimentationsQuery query) => await ResponseHandler(() => _mediator.Send(query));
    }
}
