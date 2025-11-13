using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.DTOs.Documents
{
    public record DocumentDto(
        int Id,
        string FileName,
        LeaseDocumentType FileType,
        string FilePath,
        string? Description,
        DateTime UploadAt
    )
    {

        public static DocumentDto FromEntity(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return new DocumentDto(
                document.Id,
                document.FileName,
                document.DocumentType,
                document.FilePath,
                document.Description,
                document.UploadAt
            );
        }
    }
}
