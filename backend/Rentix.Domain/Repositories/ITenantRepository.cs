using Rentix.Domain.Entities;

namespace Rentix.Domain.Repositories
{
    /// <summary>
    /// Provides methods to manage Tenant entities in the persistence layer.
    /// </summary>
    public interface ITenantRepository
    {
        /// <summary>
        /// Adds a new Tenant entity to the persistence context.
        /// </summary>
        /// <param name="tenant">The Tenant entity to add.</param>
        /// <returns>The added Tenant entity.</returns>
        Task<Tenant> AddAsync(Tenant tenant);

        /// <summary>
        /// Removes a Tenant entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tenant to remove.</param>
        /// <returns>True if the tenant was found and removed; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Gets a Tenant entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tenant.</param>
        /// <returns>The Tenant entity if found; otherwise, null.</returns> 
        Task<Tenant?> GetTenantByIdAsync(int id);

        /// <summary>
        /// Gets a Tenant entity by its email address.
        /// </summary>
        /// <param name="email">The email address of the tenant.</param>
        /// <returns>The Tenant entity if found; otherwise, null.</returns>
        Task<Tenant?> GetTenantByEmailAsync(string email);
    }
}
