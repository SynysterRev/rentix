using MediatR;

namespace Rentix.Application.RealEstate.Commands.Delete
{
    public record DeletePropertyCommand(int propertyId) : IRequest
    {
    }
}
