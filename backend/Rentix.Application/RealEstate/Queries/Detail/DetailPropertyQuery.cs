using MediatR;
using Rentix.Application.RealEstate.DTOs.Properties;

namespace Rentix.Application.RealEstate.Queries.Detail
{
    public record DetailPropertyQuery(int Id) : IRequest<PropertyDetailDto>
    {
    }
}
