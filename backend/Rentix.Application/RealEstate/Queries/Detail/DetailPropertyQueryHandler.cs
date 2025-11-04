using MediatR;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Exceptions;
using Rentix.Application.RealEstate.DTOs.Properties;

namespace Rentix.Application.RealEstate.Queries.Detail
{
    public class DetailPropertyQueryHandler(IPropertyQueries propertyQueries) : IRequestHandler<DetailPropertyQuery, PropertyDetailDto>
    {
        public async Task<PropertyDetailDto> Handle(DetailPropertyQuery request, CancellationToken cancellationToken)
        {
            var property = await propertyQueries.GetPropertyByIdAsync(request.Id);

            if(property == null)
            {
                throw new NotFoundException($"Property with ID {request.Id} does not exist");
            }
            return property;
        }
    }
}
