using Rentix.Domain.Entities;

namespace Rentix.Domain.Repositories
{
    /// <summary>
    /// Provides methods to manage Tenant entities in the persistence layer.
    /// </summary>
    public interface ITenantRepository
    {
        /// <summary>
        /// Retrieves a Tenant entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tenant.</param>
        /// <returns>The Tenant entity if found; otherwise, null.</returns>
        Task<Tenant?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a Tenant entity by email address.
        /// </summary>
        /// <param name="email">The email address of the tenant.</param>
        /// <returns>The Tenant entity if found; otherwise, null.</returns>
        Task<Tenant?> GetByEmailAsync(string email);

        /// <summary>
        /// Retrieves all Tenant entities from the database.
        /// </summary>
        /// <returns>A collection of all Tenant entities.</returns>
        Task<IEnumerable<Tenant>> GetAllAsync();

        /// <summary>
        /// Retrieves all active Tenants (those with at least one active lease).
        /// </summary>
        /// <returns>A collection of active Tenant entities.</returns>
        Task<IEnumerable<Tenant>> GetActiveTenantsAsync();

        /// <summary>
        /// Retrieves all Tenants associated with a specific lease.
        /// </summary>
        /// <param name="leaseId">The unique identifier of the lease.</param>
        /// <returns>A collection of Tenant entities associated with the specified lease.</returns>
        Task<IEnumerable<Tenant>> GetTenantsByLeaseIdAsync(int leaseId);

        /// <summary>
        /// Adds a new Tenant entity to the persistence context.
        /// </summary>
        /// <param name="tenant">The Tenant entity to add.</param>
        /// <returns>The added Tenant entity.</returns>
        Task<Tenant> AddAsync(Tenant tenant);

        /// <summary>
        /// Updates an existing Tenant entity in the persistence context.
        /// </summary>
        /// <param name="tenant">The Tenant entity to update.</param>
        void Update(Tenant tenant);

        /// <summary>
        /// Removes a Tenant entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tenant to remove.</param>
        /// <returns>True if the tenant was found and removed; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Checks if a tenant with the specified email address already exists.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if a tenant with the email exists; otherwise, false.</returns>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// Checks if a tenant with the specified email address exists, excluding a specific tenant ID.
        /// Useful for update scenarios to check email uniqueness.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <param name="excludeTenantId">The tenant ID to exclude from the check.</param>
        /// <returns>True if another tenant with the email exists; otherwise, false.</returns>
        Task<bool> ExistsByEmailAsync(string email, int excludeTenantId);
    }
}
