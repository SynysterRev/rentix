using MediatR;
using Rentix.Application.RealEstate.DTOs.Properties;

namespace Rentix.Application.RealEstate.Queries.Detail
{
    public class DetailPropertyQuery : IRequest<PropertyListDto>
    {
    }
}
