using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentix.Domain.Entities
{
    public class Lease
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The starting date of the lease is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "The end date of the lease is required")]
        [DateGreaterThan("StartDate", ErrorMessage = "The end date must be after the start date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "The rent amount is required")]
        [Precision(10, 2)]
        public decimal RentAmount { get; set; }

        [Required(ErrorMessage = "The deposit is required")]
        [Precision(10, 2)]
        public decimal Deposit { get; set; }

        [Required(ErrorMessage = "You should specified if the lease is active or not")]
        public bool IsActive { get; set; }

        [Column(TypeName = "text")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "The property id concerned by the lease is required")]
        public int PropertyId { get; set; }

        public virtual Property Property { get; set; } = null!;

        public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
    }
}
