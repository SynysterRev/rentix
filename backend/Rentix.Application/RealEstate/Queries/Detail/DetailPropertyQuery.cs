using MediatR;
using Rentix.Application.RealEstate.DTOs;

namespace Rentix.Application.RealEstate.Queries.Detail
{
    public class DetailPropertyQuery : IRequest<PropertyDto>
    {
    }
}
