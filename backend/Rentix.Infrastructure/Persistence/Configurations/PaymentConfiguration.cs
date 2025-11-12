using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rentix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentix.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(p => p.DatePaid)
                   .IsRequired();

            builder.Property(p => p.Method)
                   .HasMaxLength(50);

            builder.Property(p => p.Comment)
                   .HasMaxLength(300);

            builder.HasOne(p => p.Lease)
                   .WithMany(l => l.Payments)
                   .HasForeignKey(p => p.LeaseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
