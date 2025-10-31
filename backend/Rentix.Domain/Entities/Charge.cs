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
        public int Id { get; set; }
        public ChargeType ChargeType { get; set; }
        public string? OtherDescription { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncludedInRent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int PropertyId { get; set; }
        public virtual Property Property { get; set; } = null!;

        private Charge() { }

        public static Charge Create(
            ChargeType chargeType,
            string? otherDescription,
            decimal amount,
            bool isIncludedInRent,
            DateTime startDate,
            DateTime? endDate,
            int propertyId)
        {
            if (amount <= 0)
            {
                throw new ValidationException("The amount should be greater than 0");
            }

            if (endDate != null && endDate <= startDate)
            {
                throw new ValidationException("The end date should be greater than start date");
            }

            return new Charge
            {
                ChargeType = chargeType,
                OtherDescription = otherDescription,
                Amount = amount,
                IsIncludedInRent = isIncludedInRent,
                StartDate = startDate,
                EndDate = endDate,
                PropertyId = propertyId
            };
        }

    }
}
