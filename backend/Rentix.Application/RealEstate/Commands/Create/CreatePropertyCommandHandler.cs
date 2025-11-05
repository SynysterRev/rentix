using MediatR;
using Rentix.Application.Exceptions;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using System.Net;

namespace Rentix.Application.RealEstate.Commands.Create
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyDetailDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, IAddressRepository addressRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyDetailDto> Handle(CreatePropertyCommand command, CancellationToken cancellationToken)
        {
            Address? address = null;
            if (command.AddressId.HasValue)
            {
                address = await _addressRepository.GetByIdAsync(command.AddressId.Value);

                if (address == null)
                {
                    throw new NotFoundException($"Address with ID {command.AddressId.Value} not found");
                }
            }
            else if (command.AddressDto != null)
            {
                address = await _addressRepository.AddAsync(command.AddressDto.ToEntity());
            }

            if (address == null)
            {
                throw new InvalidOperationException("Address must be resolved before creating a property.");
            }

            var property = Property.Create(

                command.Name,
                command.MaxRent,
                command.Deposit,
                command.RentNoCharges,
                command.RentCharges,
                command.PropertyStatus,
                command.Surface,
                command.NumberRooms,
                address.Id,
                command.LandLordId
            );
            var createdProperty = await _propertyRepository.AddAsync(property);
            await _unitOfWork.SaveChangesAsync();

            createdProperty.Address = address;
            return PropertyDetailDto.FromEntity(createdProperty);
        }
    }
}
