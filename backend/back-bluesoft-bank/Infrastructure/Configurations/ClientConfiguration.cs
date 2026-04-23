using back_bluesoft_bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace back_bluesoft_bank.Infrastructure.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.IdentificationType)
            .IsRequired();

        builder.Property(c => c.IdentificationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.IdentificationNumber)
            .IsUnique();

        builder.Property(c => c.Email)
            .HasMaxLength(150);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Ignore(c => c.FullName);
    }
}
