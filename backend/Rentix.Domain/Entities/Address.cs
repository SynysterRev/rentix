using Rentix.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentix.Domain.Entities
{
    public class Address
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The street is required"), 
            MaxLength(255, ErrorMessage = "The street name should not be longer than 255 characters")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "The postal code is required"), 
            MaxLength(20, ErrorMessage = "The postal code should not be longer than 20 characters")]
        public string PostalCode {  get; set; } = string.Empty;

        [Required(ErrorMessage = "The city name is required"), 
            MaxLength(100, ErrorMessage = "The city name should not be longer than 100 characters")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "The country name is required"), 
            MaxLength(100, ErrorMessage = "The country name should not be longer than 100 characters")]
        public string Country { get; set; } = string.Empty;

        public string? Complement { get; set; }

        [NotMapped]
        public string FullAddress => $"{Street}, {PostalCode} {City}, {Country}";

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
