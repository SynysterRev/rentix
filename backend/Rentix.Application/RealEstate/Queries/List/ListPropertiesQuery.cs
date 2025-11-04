using MediatR;
using Rentix.Application.RealEstate.DTOs.Properties;

namespace Rentix.Application.RealEstate.Queries.List
{
    public record ListPropertiesQuery : IRequest<List<PropertyListDto>>
    {
    }
}
