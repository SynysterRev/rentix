using MediatR;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Application.Tenants.Commands.Create
{
    public record CreateTenantCommand : IRequest<TenantDto>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
    }
}
