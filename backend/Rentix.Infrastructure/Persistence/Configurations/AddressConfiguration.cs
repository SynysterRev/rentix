using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rentix.Domain.Entities;

namespace Rentix.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(a => a.PostalCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Country)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Complement);

            builder.HasMany(a => a.Users)
                   .WithOne(u => u.Address)
                   .HasForeignKey(u => u.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
