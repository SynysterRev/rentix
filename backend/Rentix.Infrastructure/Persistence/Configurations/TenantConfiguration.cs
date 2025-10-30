using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rentix.Domain.Entities;

namespace Rentix.Infrastructure.Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasMany(t => t.Leases)
                   .WithMany(l => l.Tenants)
                   .UsingEntity(j => j.ToTable("lease_tenant"));
        }
    }
}
