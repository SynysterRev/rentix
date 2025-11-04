using System.ComponentModel.DataAnnotations;

namespace Rentix.Domain.Entities
{
    public class Lease
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RentAmount { get; set; }
        public decimal Deposit { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }

        public int PropertyId { get; set; }
        public virtual Property Property { get; set; } = null!;

        public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();

        private Lease() { }

        public static Lease Create(
           DateTime startDate,
           DateTime endDate,
           decimal rentAmount,
           decimal deposit,
           bool isActive,
           string? notes,
           int propertyId
           )
        {
            if (endDate <= startDate)
            {
                throw new ValidationException("End date should be greater than start date");
            }

            if (rentAmount <= 0)
            {
                throw new ValidationException("The rent amount should be greater than 0");
            }

            if (deposit <= 0)
            {
                throw new ValidationException("The deposit should be greater than 0");
            }

            return new Lease
            {
                StartDate = startDate,
                EndDate = endDate,
                RentAmount = rentAmount,
                Deposit = deposit,
                IsActive = isActive,
                Notes = notes,
                PropertyId = propertyId
            };
        }
    }
}
