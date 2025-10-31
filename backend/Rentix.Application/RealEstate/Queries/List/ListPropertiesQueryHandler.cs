using MediatR;
using Rentix.Application.RealEstate.DTOs;
using Rentix.Domain.Repositories;

namespace Rentix.Application.RealEstate.Queries.List
{
    public class ListPropertiesQueryHandler(IPropertyRepository propertyRepository) : IRequestHandler<ListPropertiesQuery, List<PropertyDto>>
    {
        public async Task<List<PropertyDto>> Handle(ListPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await propertyRepository.GetAllAsync();
            return properties.Select(p => PropertyDto.FromEntity(p)).ToList();
        }
    }
}
