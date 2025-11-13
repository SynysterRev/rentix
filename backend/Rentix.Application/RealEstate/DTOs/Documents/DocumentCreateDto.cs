using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.DTOs.Documents
{
    public record DocumentCreateDto(
        LeaseDocumentType DocumentType,
        string FileName,
        string FilePath,
        string ContentType,
        long FileSize,
        int PropertyId,
        string? Description,
        DocumentEntityType? EntityType,
        int? EntityId)
    {

        /// <summary>
        /// Converts this DocumentCreateDto to an Document entity using the Document factory.
        /// </summary>
        /// <returns>An Document entity created from the DTO values.</returns>
        public Document ToEntity()
        {
            return Document.Create(DocumentType, FileName, FilePath, ContentType, FileSize, PropertyId, EntityType, EntityId, Description);
        }
    }
}
