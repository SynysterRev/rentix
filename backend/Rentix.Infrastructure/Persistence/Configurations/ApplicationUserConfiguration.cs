using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rentix.Domain.IdentityEntities;

namespace Rentix.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.AddressId)
                .IsRequired();

            builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(200);

            builder.Property(u => u.PhoneNumber)
               .IsRequired()
               .HasMaxLength(20);
        }
    }
}
