using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rentix.Domain.Entities;

namespace Rentix.Infrastructure.Persistence.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("Properties");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(p => p.MaxRent)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(p => p.RentNoCharges)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(p => p.RentCharges)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(p => p.Deposit)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(p => p.Surface)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(p => p.Status)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(p => p.NumberRooms)
                   .IsRequired();

            builder.Property(p => p.AddressId)
                   .IsRequired();

            builder.Property(p => p.LandlordId)
                   .IsRequired();

            builder.HasOne(p => p.Address)
                   .WithMany(a => a.Properties)
                   .HasForeignKey(p => p.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Landlord)
                   .WithMany(u => u.Properties)
                   .HasForeignKey(p => p.LandlordId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Documents)
                   .WithOne(d => d.Property)
                   .HasForeignKey(d => d.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Leases)
                   .WithOne(l => l.Property)
                   .HasForeignKey(l => l.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Charges)
                   .WithOne(c => c.Property)
                   .HasForeignKey(c => c.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
