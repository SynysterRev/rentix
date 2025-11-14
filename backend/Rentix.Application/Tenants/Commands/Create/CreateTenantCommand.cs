using MediatR;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Application.Tenants.Commands.Create
{
    public record CreateTenantCommand : IRequest<TenantDto>
    {
        public TenantCreateDto TenantData { get; init; } = null!;
    }
}
