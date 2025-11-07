using Rentix.Domain.Entities;

namespace Rentix.Domain.Repositories
{
    /// <summary>
    /// Provides methods to manage Address entities in the persistence layer.
    /// </summary>
    public interface IAddressRepository
    {
        /// <summary>
        /// Retrieves an Address entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the address.</param>
        /// <returns>The Address entity if found; otherwise, null.</returns>
        Task<Address?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new Address entity to the persistence context.
        /// </summary>
        /// <param name="address">The Address entity to add.</param>
        /// <returns>The added Address entity.</returns>
        Task<Address> AddAsync(Address address);

        /// <summary>
        /// Updates an existing Address entity in the persistence context.
        /// </summary>
        /// <param name="address">The Address entity to update.</param>
        void Update(Address address);

        /// <summary>
        /// Removes an Address entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the address to remove.</param>
        /// <returns>True if the address was found and removed; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);

    }
}