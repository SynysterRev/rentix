using Rentix.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentix.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Complement { get; set; }

        [NotMapped]
        public string FullAddress => $"{Street}, {PostalCode} {City}, {Country}";

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();

        private Address() { }

        public static Address Create(
            string street,
            string postalCode,
            string city,
            string country,
            string? complement)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ValidationException("Street is required");
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ValidationException("City is required");
            }

            if (string.IsNullOrWhiteSpace(postalCode))
            {
                throw new ValidationException("Postal code is required");
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ValidationException("Country is required");
            }

            return new Address
            {
                City = city,
                Country = country,
                Complement = complement,
                Street = street,
                PostalCode = postalCode
            };
        }
    }
}
