using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;

namespace Rentix.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implements the ITenantRepository interface for managing Tenant entities using Entity Framework Core.
    /// </summary>
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the TenantRepository class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        public TenantRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a Tenant entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tenant.</param>
        /// <returns>The Tenant entity if found; otherwise, null.</returns>
        public async Task<Tenant?> GetByIdAsync(int id)
        {
            return await _dbContext.Tenants.FindAsync(id);
        }

        /// <summary>
        /// Retrieves a Tenant entity by email address.
        /// </summary>
        /// <param name="email">The email address of the tenant.</param>
        /// <returns>The Tenant entity if found; otherwise, null.</returns>
        public async Task<Tenant?> GetByEmailAsync(string email)
        {
            return await _dbContext.Tenants
                .FirstOrDefaultAsync(t => t.Email.Value == email);
        }

        /// <summary>
        /// Retrieves all Tenant entities from the database.
        /// </summary>
        /// <returns>A collection of all Tenant entities.</returns>
        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _dbContext.Tenants.ToListAsync();
        }

        /// <summary>
        /// Retrieves all active Tenants (those with at least one active lease).
        /// </summary>
        /// <returns>A collection of active Tenant entities.</returns>
        public async Task<IEnumerable<Tenant>> GetActiveTenantsAsync()
        {
            return await _dbContext.Tenants
                .Where(t => t.Leases.Any(l => l.IsActive))
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all Tenants associated with a specific lease.
        /// </summary>
        /// <param name="leaseId">The unique identifier of the lease.</param>
        /// <returns>A collection of Tenant entities associated with the specified lease.</returns>
        public async Task<IEnumerable<Tenant>> GetTenantsByLeaseIdAsync(int leaseId)
        {
            return await _dbContext.Tenants
                .Where(t => t.Leases.Any(l => l.Id == leaseId))
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new Tenant entity to the persistence context.
        /// </summary>
        /// <param name="tenant">The Tenant entity to add.</param>
        /// <returns>The added Tenant entity.</returns>
        public async Task<Tenant> AddAsync(Tenant tenant)
        {
            await _dbContext.Tenants.AddAsync(tenant);
            return tenant;
        }

        /// <summary>
        /// Updates an existing Tenant entity in the persistence context.
        /// </summary>
        /// <param name="tenant">The Tenant entity to update.</param>
        public void Update(Tenant tenant)
        {
            _dbContext.Tenants.Update(tenant);
        }

        /// <summary>
        /// Removes a Tenant entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tenant to remove.</param>
        /// <returns>True if the tenant was found and removed; otherwise, false.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var tenant = await _dbContext.Tenants.FindAsync(id);
            
            if (tenant == null)
            {
                return false;
            }

            _dbContext.Tenants.Remove(tenant);
            return true;
        }

        /// <summary>
        /// Checks if a tenant with the specified email address already exists.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if a tenant with the email exists; otherwise, false.</returns>
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Tenants
                .AnyAsync(t => t.Email.Value == email);
        }

        /// <summary>
        /// Checks if a tenant with the specified email address exists, excluding a specific tenant ID.
        /// Useful for update scenarios to check email uniqueness.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <param name="excludeTenantId">The tenant ID to exclude from the check.</param>
        /// <returns>True if another tenant with the email exists; otherwise, false.</returns>
        public async Task<bool> ExistsByEmailAsync(string email, int excludeTenantId)
        {
            return await _dbContext.Tenants
                .AnyAsync(t => t.Email.Value == email && t.Id != excludeTenantId);
        }
    }
}
