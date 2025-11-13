using Rentix.Domain.Entities;

namespace Rentix.Domain.Repositories
{
    /// <summary>
    /// Provides methods to manage Document entities in the persistence layer.
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Retrieves a Document entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the document.</param>
        /// <returns>The Document entity if found; otherwise, null.</returns>
        Task<Document?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new Document entity to the persistence context.
        /// </summary>
        /// <param name="document">The Document entity to add.</param>
        /// <returns>The added Document entity.</returns>
        Task<Document> AddAsync(Document document);

        /// <summary>
        /// Removes a Document entity by its unique identifier.</summary>
        /// <param name="id">The unique identifier of the document to remove.</param>
        /// <returns>True if the document was found and removed; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Retrieves all documents attached to a given property.
        /// </summary>
        /// <param name="propertyId">Property identifier.</param>
        /// <returns>List of documents for the property.</returns>
        Task<List<Document>> GetByPropertyIdAsync(int propertyId);

        /// <summary>
        /// Retrieves documents linked to a specific entity type and id (e.g., Lease, Tenant, Charge).
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="entityId">The entity id.</param>
        /// <returns>List of matching documents.</returns>
        Task<List<Document>> GetByEntityAsync(DocumentEntityType entityType, int entityId);
    }
}
