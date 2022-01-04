using Bank.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IBankContext _bankContext;

        public AccountController(IBankContext bankContext)
        {
            _bankContext = bankContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            return Ok(await _bankContext.AccountBalances.ToListAsync());
        }
    }
}
