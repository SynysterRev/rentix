using Rentix.Domain.IdentityEntities;

namespace Rentix.Domain.Entities
{
    public enum PropertyStatus
    {
        Available,
        Rented,
        UnderMaintenance,
        Unavailable
    }

    public class Property : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal MaxRent { get; set; }
        public PropertyStatus Status { get; set; }
        public decimal Surface { get; set; }
        public int NumberRooms { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; } = null!;

        public Guid LandlordId { get; set; }
        public virtual ApplicationUser Landlord { get; set; } = null!;

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<Document> Leases { get; set; } = new List<Document>();
        public virtual ICollection<Charge> Charges { get; set; } = new List<Charge>();

        private Property() { }

        public static Property Create(
            string name,
            decimal maxRent,
            PropertyStatus status,
            decimal surface,
            int numberRooms,
            int addressId,
            Guid landlordId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Property name is required.");

            if (maxRent <= 0)
                throw new ArgumentException("Max rent must be positive.");

            if (surface <= 0)
                throw new ArgumentException("Surface must be positive.");

            if (numberRooms < 1 || numberRooms > 150)
                throw new ArgumentException("Number of rooms must be between 1 and 150.");

            return new Property
            {
                Name = name,
                MaxRent = maxRent,
                Status = status,
                Surface = surface,
                NumberRooms = numberRooms,
                AddressId = addressId,
                LandlordId = landlordId
            };
        }
    }
}
