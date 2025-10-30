using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace Rentix.Domain.Entities
{
    public enum ChargeType
    {
        PropertyTax,           // Taxe foncière
        CondominiumFees,       // Charges de copropriété
        Insurance,             // Assurance
        Maintenance,           // Entretien courant
        Repairs,               // Travaux et réparations
        Electricity,           // Électricité
        Water,                 // Eau
        Heating,               // Chauffage
        PropertyManagement,    // Gestion locative
        Other                  // Autre
    }

    public class Charge
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The charge type is required")]
        public ChargeType ChargeType { get; set; }

        [StringLength(100, ErrorMessage = "The description should not be longer than 100 characters")]
        public string? OtherDescription { get; set; }


        [Required(ErrorMessage = "The amount of the charge is required")]
        [Precision(10, 2)]
        [Range(0.01, 100000.00, ErrorMessage = "The amount must be between 0.01 and 100,000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "You should precise whether or not this charge is included in the rend")]
        public bool IsIncludedInRent {  get; set; }

        [Required(ErrorMessage = "The starting date of the charge is required")]
        public DateTime StartDate { get; set; }

        [DateGreaterThan("StartDate", ErrorMessage = "The end date must be after the start date")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "The property id is required")]
        public int PropertyId { get; set; }

        public virtual Property Property { get; set; } = null!;
    }
}
