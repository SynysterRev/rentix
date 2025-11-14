using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Application.Common.Interfaces.Queries
{
    /// <summary>
    /// Provides query methods for retrieving Tenant data.
    /// </summary>
    public interface ITenantQueries
    {
        /// <summary>
        /// Retrieves a list of all tenants.
        /// </summary>
        /// <returns>
        /// An asynchronous task returning a list of <see cref="TenantDto"/>
        /// representing all tenants in the database.
        /// </returns>
        Task<List<TenantDto>> GetAllTenantsAsync();

        /// <summary>
        /// Retrieves a tenant by their unique identifier.
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant.</param>
        /// <returns>
        /// An asynchronous task returning a <see cref="TenantDto"/>, if any,
        /// containing information about the specified tenant.
        /// </returns>
        Task<TenantDto?> GetTenantByIdAsync(int tenantId);

        /// <summary>
        /// Retrieves a tenant by their email address.
        /// </summary>
        /// <param name="email">The email address of the tenant.</param>
        /// <returns>
        /// An asynchronous task returning a <see cref="TenantDto"/>, if any,
        /// containing information about the tenant with the specified email.
        /// </returns>
        Task<TenantDto?> GetTenantByEmailAsync(string email);

        /// <summary>
        /// Retrieves all active tenants (those with at least one active lease).
        /// </summary>
        /// <returns>
        /// An asynchronous task returning a list of <see cref="TenantDto"/>
        /// representing all active tenants.
        /// </returns>
        Task<List<TenantDto>> GetActiveTenantsAsync();

        /// <summary>
        /// Retrieves all tenants associated with a specific lease.
        /// </summary>
        /// <param name="leaseId">The unique identifier of the lease.</param>
        /// <returns>
        /// An asynchronous task returning a list of <see cref="TenantDto"/>
        /// representing all tenants associated with the specified lease.
        /// </returns>
        Task<List<TenantDto>> GetTenantsByLeaseIdAsync(int leaseId);

        /// <summary>
        /// Checks if a tenant with the specified email address already exists.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>
        /// An asynchronous task returning <c>true</c> if a tenant with the email exists; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// Checks if a tenant with the specified email address exists, excluding a specific tenant ID.
        /// Useful for update scenarios to check email uniqueness.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <param name="excludeTenantId">The tenant ID to exclude from the check.</param>
        /// <returns>
        /// An asynchronous task returning <c>true</c> if another tenant with the email exists; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> ExistsByEmailAsync(string email, int excludeTenantId);
    }
}
