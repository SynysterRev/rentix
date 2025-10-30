using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Entities;
using Rentix.Domain.IdentityEntities;

namespace Rentix.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<Document> Leases { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Document>(entity =>
            {
                entity.Property(e => e.Notes)
                    .HasColumnType("text");

                entity.HasMany(l => l.Tenants)
                    .WithMany(t => t.Leases)
                    .UsingEntity(j => j.ToTable("lease_tenant"));

                entity.HasOne(l => l.Property)
                    .WithMany(p => p.Leases)
                    .HasForeignKey(l => l.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Deposit).HasPrecision(10, 2);
                entity.Property(p => p.RentAmount).HasPrecision(10, 2);
            });

            builder.Entity<Document>()
                .HasOne(d => d.Property)
                .WithMany(p => p.Documents)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Charge>()
                .HasOne(c => c.Property)
                .WithMany(p => p.Charges)
                .HasForeignKey(c => c.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Address)
                .WithMany(a => a.Users)
                .HasForeignKey(u =>  u.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                if (entry.State == EntityState.Added)
                    entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
