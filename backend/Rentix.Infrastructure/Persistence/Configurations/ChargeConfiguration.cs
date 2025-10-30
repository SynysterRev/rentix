using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rentix.Domain.Entities;

namespace Rentix.Infrastructure.Persistence.Configurations
{
    public class ChargeConfiguration : IEntityTypeConfiguration<Charge>
    {
        public void Configure(EntityTypeBuilder<Charge> builder)
        {
            builder.ToTable("Charges");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ChargeType)
                   .IsRequired();

            builder.Property(c => c.OtherDescription)
                   .HasMaxLength(100);

            builder.Property(c => c.Amount)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(c => c.IsIncludedInRent)
                   .IsRequired();

            builder.Property(c => c.StartDate)
                   .IsRequired();

            builder.Property(c => c.EndDate);

            builder.HasOne(c => c.Property)
                   .WithMany(p => p.Charges)
                   .HasForeignKey(c => c.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
