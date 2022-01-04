using Bank.Api.Infrastructure.Base;
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

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            return Ok();
        }
    }
}
