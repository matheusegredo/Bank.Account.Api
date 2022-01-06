using Bank.Api.Infrastructure.Base;
using Bank.Application.Commands.Accounts.Authentication;
using Bank.Application.Commands.Accounts.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostAccountCommand command) => await ResponseHandler(() => _mediator.Send(command));

        [HttpPost("authentication")]
        public async Task<IActionResult> Authentication([FromBody] AuthenticationAccountCommand command) => await ResponseHandler(() => _mediator.Send(command));
    }
}
