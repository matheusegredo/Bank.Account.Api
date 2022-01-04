using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Persistence.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");

            builder.Property(p => p.AccountId)                
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ClientId)
                .IsRequired();

            builder.Property(p => p.AccountNumber)
                .IsRequired()
                .HasColumnType("varchar(8)")
                .HasMaxLength(8);

            builder.Property(p => p.CreatedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.ModifiedOn)
                .HasColumnType("datetime");

            builder.HasOne(p => p.Client)
                .WithOne(p => p.Account)
                .HasForeignKey<Account>(p => p.ClientId);
        }
    }
}
