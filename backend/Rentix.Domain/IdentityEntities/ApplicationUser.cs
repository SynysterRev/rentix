using Microsoft.AspNetCore.Identity;
using Rentix.Domain.Entities;
using Rentix.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentix.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public int AddressId { get; set; }
        public virtual Address Address { get; set; } = null!;

        [NotMapped]
        public Email EmailValue
        {
            get => Email is null ? throw new InvalidOperationException("Email not set") : ValueObjects.Email.Create(Email);
            set => Email = value.Value;
        }

        [NotMapped]
        public Phone PhoneValue
        {
            get => PhoneNumber is null ? throw new InvalidOperationException("Phone not set") : Phone.Create(PhoneNumber);
            set => PhoneNumber = value.Value;
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
