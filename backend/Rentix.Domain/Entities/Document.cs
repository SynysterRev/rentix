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

    public class Document
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public LeaseDocumentType DocumentType { get; set; }

        [Required, MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string FileType { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Description {  get; set; } = string.Empty;

        [Required]
        public int PropertyId { get; set; }

        public virtual Property Property { get; set; } = null!;
    }
}
