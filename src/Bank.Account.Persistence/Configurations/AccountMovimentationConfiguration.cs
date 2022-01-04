using Bank.Data;
using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Persistence.Configurations
{
    public class AccountMovimentationConfiguration : IEntityTypeConfiguration<AccountMovimentation>
    {
        public void Configure(EntityTypeBuilder<AccountMovimentation> builder)
        {
            builder.ToTable("AccountMovimentation");

            builder.Property(p => p.AccountMovimentationId)
               .ValueGeneratedOnAdd();

            builder.Property(p => p.AccountId)
                .IsRequired();

            builder.Property(p => p.Value)
                .HasColumnType("decimal(9,2)")
                .HasPrecision(2);

            builder.Property(p => p.Type)
                .HasMaxLength(7)
                .IsRequired()
                .HasColumnType("varchar(7)")
                .HasConversion(
                    v => v.ToString(),
                    v => (MovimentationType)Enum.Parse(typeof(MovimentationType), v));

            builder.Property(p => p.CreatedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.ModifiedOn)
                .HasColumnType("datetime");

            builder.HasOne(p => p.Account)
                .WithMany(p => p.Movimentations)
                .HasForeignKey(p => p.AccountId);
        }
    }
}
