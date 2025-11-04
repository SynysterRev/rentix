using MediatR;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;

namespace Rentix.Application.RealEstate.Commands.Create
{
    public class CreatePropertyCommandHandler(IPropertyRepository propertyRepository) : IRequestHandler<CreatePropertyCommand, int>
    {
        public async Task<int> Handle(CreatePropertyCommand command, CancellationToken cancellationToken)
        {
            var property = Property.Create(

                command.Name,
                command.MaxRent,
                command.Deposit,
                command.RentNoCharges,
                command.RentCharges,
                command.PropertyStatus,
                command.Surface,
                command.NumberRooms,
                command.AddressId,
                command.LandLordId
            );
            var createdProperty = await propertyRepository.AddAsync(property);
            return createdProperty.Id;
        }
    }
}
