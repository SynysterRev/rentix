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
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
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
                .Where(e => (e.Entity is BaseEntity || e.Entity is ApplicationUser) &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                switch (entry.Entity)
                {
                    case BaseEntity baseEntity:
                        if (entry.State == EntityState.Added)
                            baseEntity.CreatedAt = DateTime.UtcNow;
                        baseEntity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case ApplicationUser appUser:
                        if (entry.State == EntityState.Added)
                            appUser.CreatedAt = DateTime.UtcNow;
                        appUser.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
