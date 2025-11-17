using MediatR;
using Rentix.Application.Common.Interfaces.Queries;
using Rentix.Application.Tenants.DTOs.Tenants;

namespace Rentix.Application.Tenants.Queries.List
{
    public class ListTenantsQueryHandler : IRequestHandler<ListTenantsQuery, List<TenantDto>>
    {
        private readonly ITenantQueries _tenantsQueries;

        public ListTenantsQueryHandler(ITenantQueries tenantsQueries)
        {
            _tenantsQueries = tenantsQueries;
        }

        public async Task<List<TenantDto>> Handle(ListTenantsQuery request, CancellationToken cancellationToken)
        {
            return await _tenantsQueries.GetAllTenantsAsync();
        }
    }
}
