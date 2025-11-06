using Rentix.Domain.Entities;

namespace Rentix.Domain.Repositories
{
    public interface IPropertyRepository
    {
        /// <summary>
        /// Adds a new property to the database.
        /// </summary>
        /// <param name="property">The property entity to add.</param>
        /// <returns>An asynchronous task returning the added <see cref="Property"/> with its generated ID.</returns>
        public Task<Property> AddAsync(Property property);

        /// <summary>
        /// Retrieves an existing property in the database.
        /// </summary>
        /// <param name="id">The id of the property entity.</param>
        /// <returns>An asynchronous task returning the founded <see cref="Property"/> if any, otherwise null</returns>
        public Task<Property?> GetPropertyByIdAsync(int id);

        /// <summary>
        /// Deletes a property with the specified ID from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the property to delete.</param>
        /// <returns>An asynchronous task returning <c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
        public Task<bool> DeleteAsync(int id);

    }
}
