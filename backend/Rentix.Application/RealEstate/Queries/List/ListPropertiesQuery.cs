using MediatR;
using Rentix.Application.RealEstate.DTOs;

namespace Rentix.Application.RealEstate.Queries.List
{
    public record ListPropertiesQuery : IRequest<List<PropertyDto>>
    {
    }
}
