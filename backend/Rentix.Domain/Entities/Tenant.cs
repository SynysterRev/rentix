using System.ComponentModel.DataAnnotations;

namespace Rentix.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The first name is required"), 
            MaxLength(100, ErrorMessage = "The first name should not be longer than 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The last name is required"), 
            MaxLength(100, ErrorMessage = "The last name should not be longer than 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The email is required"), 
            MaxLength(200, ErrorMessage = "The email not be longer than 200 characters")]
        [EmailAddress(ErrorMessage = "The email format is incorrect")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The phone number is required"), 
            MaxLength(20, ErrorMessage = "The phone number should not be longer than 20 characters")]
        [Phone(ErrorMessage = "The phone number format is incorrect")]
        public string Phone { get; set; } = string.Empty;

        public virtual ICollection<Document> Leases { get; set; } = new List<Document>();
    }
}
