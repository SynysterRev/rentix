using Rentix.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Rentix.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Email Email { get; set; } = null!;
        public Phone Phone { get; set; } = null!;

        public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

        private Tenant() { }

        public static Tenant Create(
            string firstName,
            string lastName,
            Email email,
            Phone phone)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ValidationException("First name is required");
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ValidationException("Last name is required");
            }
            return new Tenant
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone
            };
        }
    }
}
