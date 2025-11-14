using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.API.Models
{
    public record LeaseRequestDto
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public decimal RentAmount { get; init; }
        public decimal ChargesAmount { get; init; }
        public decimal Deposit { get; init; }
        public bool IsActive { get; init; }
        public string? Notes { get; init; }
        public IFormFile LeaseDocument { get; init; } = null!;
        public List<TenantCreateDto> Tenants { get; init; } = new();
    }
}
