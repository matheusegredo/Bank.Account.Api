using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bank.Persistence.Interfaces
{
    public interface IBankContext
    {
        DbSet<Account> Accounts { get; set; }

        DbSet<AccountBalance> AccountBalances { get; set; }

        DbSet<AccountMovimentation> AccountMovimentations { get; set; }

        DbSet<Client> Clients { get; set; }

        IModel Model { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
