using Bank.Application.Interfaces;
using Bank.Data;
using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Persistence
{
    public class BankContext : DbContext, IBankContext
    {
        public BankContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountBalance> AccountBalances { get; set; }

        public DbSet<AccountMovimentation> AccountMovimentations { get; set; }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasKey(p => p.AccountId);

            modelBuilder.Entity<AccountBalance>()
                .HasKey(p => p.AccountBalanceId);

            modelBuilder.Entity<AccountMovimentation>()
                .HasKey(p => p.AccountMovimentationId);

            modelBuilder.Entity<Client>()
                .HasKey(p => p.ClientId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Auditable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.ModifiedOn = entry.Entity.CreatedOn = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedOn = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
