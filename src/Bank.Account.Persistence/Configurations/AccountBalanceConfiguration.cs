using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Persistence.Configurations
{
    public class AccountBalanceConfiguration : IEntityTypeConfiguration<AccountBalance>
    {
        public void Configure(EntityTypeBuilder<AccountBalance> builder)
        {
            builder.ToTable("AccountBalance");

            builder.Property(p => p.AccountBalanceId)
               .ValueGeneratedOnAdd();

            builder.Property(p => p.AccountId)
                .IsRequired();

            builder.Property(p => p.Value)
                .HasColumnType("decimal(9,2)")
                .HasPrecision(2);

            builder.Property(p => p.LastTimeChanged)
                .HasColumnType("datetime");            

            builder.HasOne(p => p.Account)
                .WithOne(p => p.AccountBalance)
                .HasForeignKey<AccountBalance>(p => p.AccountId);
        }
    }
}
