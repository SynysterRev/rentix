using MediatR;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.RealEstate.DTOs.Properties;

namespace Rentix.Application.RealEstate.Queries.List
{
    public class ListPropertiesQueryHandler(IPropertyQueries propertyQueries) : IRequestHandler<ListPropertiesQuery, List<PropertyListDto>>
    {
        public async Task<List<PropertyListDto>> Handle(ListPropertiesQuery request, CancellationToken cancellationToken)
        {
            return await propertyQueries.GetPropertyListAsync();
        }
    }
}
