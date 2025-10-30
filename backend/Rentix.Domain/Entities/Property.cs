using Microsoft.EntityFrameworkCore;
using Rentix.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;

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
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The property name is required"),
            MaxLength(255, ErrorMessage = "The property name should not be longer than 255 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The max rent is required")]
        [Precision(10, 2)]
        public decimal MaxRent { get; set; }

        [Required(ErrorMessage = "The property status is required")]
        public PropertyStatus Status { get; set; }

        [Required(ErrorMessage = "The property's surface is required")]
        [Precision(10, 2)]
        public decimal Surface { get; set; }

        [Required(ErrorMessage = "The number of rooms is required")]
        [Range(1, 150, ErrorMessage = "The number of rooms should be between 1 and 150")]
        public int NumberRooms { get; set; }

        [Required(ErrorMessage = "The address id is required")]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; } = null!;

        [Required(ErrorMessage = "The user id is required")]
        public Guid LandlordId { get; set; }

        public virtual ApplicationUser Landlord { get; set; } = null!;

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();
        public virtual ICollection<Charge> Charges { get; set; } = new List<Charge>();
    }
}
