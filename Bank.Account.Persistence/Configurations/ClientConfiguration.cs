using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Persistence.Configurations
{
    internal class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");

            builder.Property(p => p.ClientId)                
                .ValueGeneratedOnAdd();

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasColumnType("varchar(200)")
                .HasMaxLength(200);

            builder.Property(p => p.LastName)
               .IsRequired()
               .HasColumnType("varchar(200)")
               .HasMaxLength(200);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnType("varchar(150)")
                .HasMaxLength(150);

            builder.Property(p => p.CreatedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.ModifiedOn)
                .HasColumnType("datetime");

            builder.HasOne(p => p.Account)
                .WithOne(p => p.Client)
                .HasForeignKey<Account>(p => p.ClientId);
        }
    }
}
