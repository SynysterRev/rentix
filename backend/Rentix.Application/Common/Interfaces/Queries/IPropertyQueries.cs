using Rentix.Application.RealEstate.DTOs.Properties;

namespace Rentix.Application.Common.Interfaces.Queries
{
    public interface IPropertyQueries
    {
        /// <summary>
        /// Retrieves a list of properties with their main details, including
        /// name, total rent amount (with charges), active tenants,
        /// status, and associated address.
        /// </summary>
        /// <returns>
        /// An asynchronous task returning a list of <see cref="PropertyListDto"/>
        /// representing the properties available in the database.
        /// </returns>
        public Task<List<PropertyListDto>> GetPropertyListAsync();

        /// <summary>
        /// Retrieves detailed information about a specific property
        /// based on its unique identifier.
        /// </summary>
        /// <param name="propertyId">
        /// The unique identifier of the property to retrieve.
        /// </param>
        /// <returns>
        /// An asynchronous task returning a <see cref="PropertyDetailDto"/>, if any,
        /// containing detailed information about the specified property.
        /// </returns>
        public Task<PropertyDetailDto?> GetPropertyByIdAsync(int propertyId);

        /// <summary>
        /// Checks whether a property with the specified ID exists in the database.
        /// </summary>
        /// <param name="propertyId">
        /// The unique identifier of the property to check.
        /// </param>
        /// <returns>
        /// An asynchronous task returning <c>true</c> if the property exists; otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> ExistsAsync(int propertyId);
    }
}
