using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rentix.Domain.Entities;

namespace Rentix.Infrastructure.Persistence.Configurations
{
    public class LeaseConfiguration : IEntityTypeConfiguration<Lease>
    {
        public void Configure(EntityTypeBuilder<Lease> builder)
        {
            builder.ToTable("Leases");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.StartDate)
                   .IsRequired();

            builder.Property(l => l.EndDate)
                   .IsRequired();

            builder.Property(l => l.RentAmount)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(l => l.Deposit)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(l => l.IsActive)
                   .IsRequired();

            builder.Property(l => l.Notes)
                   .HasColumnType("text");

            // Many-to-many with Tenant handled in TenantConfiguration

            builder.HasOne(l => l.Property)
                   .WithMany(p => p.Leases)
                   .HasForeignKey(l => l.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
