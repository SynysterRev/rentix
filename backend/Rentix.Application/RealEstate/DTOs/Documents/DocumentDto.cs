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
    );
}
