using Rentix.Domain.Entities;

namespace Rentix.Domain.Repositories
{
    /// <summary>
    /// Provides methods to manage Lease entities in the persistence layer.
    /// </summary>
    public interface ILeaseRepository
    {
        /// <summary>
        /// Retrieves a Lease entity by its unique identifier, including related entities.
        /// </summary>
        /// <param name="id">The unique identifier of the lease.</param>
        /// <returns>The Lease entity if found; otherwise, null.</returns>
        Task<Lease?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new Lease entity to the persistence context.
        /// </summary>
        /// <param name="lease">The Lease entity to add.</param>
        /// <returns>The added Lease entity.</returns>
        Task<Lease> AddAsync(Lease lease);

        /// <summary>
        /// Removes a Lease entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the lease to remove.</param>
        /// <returns>True if the lease was found and removed; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Retrieves all Lease entities, including related entities.
        /// </summary>
        /// <returns>List of Lease entities.</returns>
        Task<List<Lease>> GetAllAsync();
    }
}
