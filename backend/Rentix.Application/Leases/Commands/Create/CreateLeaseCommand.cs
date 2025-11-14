using MediatR;
using Rentix.Application.Leases.DTOs;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Application.Leases.Commands.Create
{
    public record CreateLeaseCommand : IRequest<LeaseDto>
    {
        public int PropertyId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public decimal RentAmount { get; init; }
        public decimal ChargesAmount { get; init; }
        public decimal Deposit { get; init; }
        public bool IsActive { get; init; }
        public string? Notes { get; init; }
        public Stream FileStream { get; init; } = null!;
        public string FileName { get; init; } = null!;
        public string ContentType { get; init; } = null!;
        public long FileSizeInBytes { get; init; }
        public List<TenantCreateDto> Tenants { get; init; } = new();
    }
}
