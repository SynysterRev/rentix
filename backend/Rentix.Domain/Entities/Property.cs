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
        public decimal RentNoCharges { get; set; }
        public decimal RentCharges { get; set; }
        public decimal Deposit { get; set; }
        public decimal MaxRent { get; set; }
        public PropertyStatus Status { get; set; }
        public decimal Surface { get; set; }
        public int NumberRooms { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; } = null!;

        public Guid LandlordId { get; set; }
        public virtual ApplicationUser Landlord { get; set; } = null!;

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();
        public virtual ICollection<Charge> Charges { get; set; } = new List<Charge>();

        public decimal TotalRent => RentNoCharges + RentCharges;

        private Property() { }

        public static Property Create(
            string name,
            decimal maxRent,
            decimal deposit,
            decimal rentNoCharges,
            decimal rentCharges,
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

            if (deposit <= 0)
                throw new ArgumentException("Deposit must be positive.");

            if (rentNoCharges <= 0)
                throw new ArgumentException("Rent without charges must be positive.");

            if (rentCharges <= 0)
                throw new ArgumentException("Charges included in rent must be positive.");

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
                RentCharges = rentCharges,
                RentNoCharges = rentNoCharges,
                Deposit = deposit,
                LandlordId = landlordId
            };
        }

        public void UpdateDetails(
            string? name = null,
            decimal? deposit = null,
            decimal? rentNoCharges = null,
            decimal? rentCharges = null,
            PropertyStatus? status = null,
            decimal? surface = null,
            int? numberRooms = null,
            int? addressId = null)
        {
            if (name is not null)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Property name cannot be empty.");
                }
                Name = name;
            }
            if (deposit is not null)
            {
                if (deposit <= 0)
                {
                    throw new ArgumentException("Deposit must be positive.");
                }
                Deposit = deposit.Value;
            }
            if (rentNoCharges is not null)
            {
                if (rentNoCharges <= 0)
                {
                    throw new ArgumentException("Rent without charges must be positive.");
                }
                RentNoCharges = rentNoCharges.Value;
            }
            if (rentCharges is not null)
            {
                if (rentCharges <= 0)
                {
                    throw new ArgumentException("Charges included in rent must be positive.");
                }
                RentCharges = rentCharges.Value;
            }
            if (status is not null)
            {
                Status = status.Value;
            }
            if (surface is not null)
            {
                if (surface <= 0)
                {
                    throw new ArgumentException("Surface must be positive.");
                }
                Surface = surface.Value;
            }
            if (numberRooms is not null)
            {
                if (numberRooms < 1 || numberRooms > 150)
                {
                    throw new ArgumentException("Number of rooms must be between 1 and 150.");
                }
                NumberRooms = numberRooms.Value;
            }
            if (addressId is not null)
            {
                AddressId = addressId.Value;
            }
        }
    }
}
