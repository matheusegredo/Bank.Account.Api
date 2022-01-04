using Bank.Api.Infrastructure.Base;
using Bank.Application.Commands.Clients.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("api/client")]
    public class ClientController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostClientCommand command) => await ResponseHandler(() => _mediator.Send(command));        
    }
}
