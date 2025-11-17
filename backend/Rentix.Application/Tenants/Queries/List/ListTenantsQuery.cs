using MediatR;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Application.Tenants.Queries.List
{
    public record ListTenantsQuery : IRequest<List<TenantDto>>
    {
    }
}
