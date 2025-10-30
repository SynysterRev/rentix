using Microsoft.AspNetCore.Identity;
using Rentix.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Rentix.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required(ErrorMessage = "The address id is required")]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; } = null!;

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
