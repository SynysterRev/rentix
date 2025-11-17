using Rentix.Domain.Entities;

namespace Rentix.Application.RealEstate.DTOs.Documents
{
    public record DocumentDto(
        int Id,
        string FileName,
        LeaseDocumentType FileType,
        string? Description,
        DateTime UploadAt,
        string DownloadUrl
    )
    {

        public static DocumentDto FromEntity(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            string downloadUrl = $"/api/documents/{document.Id}/download";

            return new DocumentDto(
                document.Id,
                document.FileName,
                document.DocumentType,
                document.Description,
                document.UploadAt,
                downloadUrl
            );
        }
    }
}
