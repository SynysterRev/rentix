using System.ComponentModel.DataAnnotations;

namespace Rentix.Domain.Entities
{
    public enum LeaseDocumentType
    {
        /// <summary>
        /// Energy Performance Certificate (EPC) - (Equivalent to DPE in France, mandatory).
        /// </summary>
        EPC,

        /// <summary>
        /// Lead Risk Exposure Report (Mandatory if construction was before 1978 in the US, or before 1949 in France/CREP).
        /// </summary>
        LeadReport,

        /// <summary>
        /// Natural Risks and Pollution Report (Environmental risks disclosure).
        /// </summary>
        EnvironmentalRisksReport,

        /// <summary>
        /// Asbestos Report (Required for specific building types/ages).
        /// </summary>
        AsbestosReport,

        /// <summary>
        /// Electrical Installation Diagnosis (if installation is old).
        /// </summary>
        ElectricalDiagnosis,

        /// <summary>
        /// Gas Installation Diagnosis (if installation is old).
        /// </summary>
        GasDiagnosis,

        /// <summary>
        /// Certificate of habitable living space (e.g., Carrez or Boutin law surface area).
        /// </summary>
        HabitableSurfaceCertificate,

        /// <summary>
        /// The signed Lease/Rental Agreement (The main contract document).
        /// </summary>
        LeaseAgreement,

        /// <summary>
        /// Detailed inventory of furniture and fixtures (for furnished rentals).
        /// </summary>
        FurnitureInventory,

        /// <summary>
        /// Property condition report at the time of move-in (Check-in/Move-in Inspection Report).
        /// </summary>
        MoveInInspectionReport,

        /// <summary>
        /// Condominium/HOA Bylaws and Rules.
        /// </summary>
        HOARules,
    }

    public enum DocumentEntityType
    {
        Property,
        Lease,
        Tenant,
        Charge
    }

    public class Document
    {
        public int Id { get; set; }
        public LeaseDocumentType DocumentType { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadAt { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; } = string.Empty;
        public long FileSizeInBytes { get; set; }

        public int PropertyId { get; set; }
        public virtual Property Property { get; set; } = null!;

        public DocumentEntityType? EntityType { get; set; }
        public int? EntityId { get; set; }

        private Document() { }

        public static Document Create(
            LeaseDocumentType documentType,
            string fileName,
            string filePath,
            string contentType,
            long fileSizeInBytes,
            int propertyId,
            DocumentEntityType? EntityType,
            int? EntityId,
            string? description)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ValidationException("The file name is required");
            }

            //if (string.IsNullOrWhiteSpace(filePath))
            //{
            //    throw new ValidationException("The file path is required");
            //}

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ValidationException("The content type is required");
            }

            if (fileSizeInBytes <= 0)
            {
                throw new ValidationException("The file size must be greater than zero");
            }

            return new Document
            {
                Description = description,
                PropertyId = propertyId,
                FilePath = filePath,
                DocumentType = documentType,
                FileName = fileName,
                EntityType = EntityType,
                EntityId = EntityId,
                FileSizeInBytes = fileSizeInBytes,
                ContentType = contentType
            };
        }
    }
}
