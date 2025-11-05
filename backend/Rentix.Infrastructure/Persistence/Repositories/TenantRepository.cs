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
    }
}
