using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Interfaces
{
    public interface IBankContext
    {
        DbSet<Account> Accounts { get; set; }

        DbSet<AccountBalance> AccountBalances { get; set; }

        DbSet<AccountMovimentation> AccountMovimentations { get; set; }

        DbSet<Client> Clients { get; set; }
    }
}
