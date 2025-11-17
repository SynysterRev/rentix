using MediatR;
using Microsoft.Extensions.Logging;
using Rentix.Application.Exceptions;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.Mappers;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;

namespace Rentix.Application.RealEstate.Commands.Create.Property
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyDetailDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPropertyMapper _propertyMapper;
        private readonly ILogger<CreatePropertyCommandHandler> _logger;

        public CreatePropertyCommandHandler(
            IPropertyRepository propertyRepository,
            IAddressRepository addressRepository,
            IUnitOfWork unitOfWork,
            IPropertyMapper propertyMapper,
            ILogger<CreatePropertyCommandHandler> logger)
        {
            _propertyRepository = propertyRepository;
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _propertyMapper = propertyMapper;
            _logger = logger;
        }

        public async Task<PropertyDetailDto> Handle(CreatePropertyCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
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
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                if (address == null)
                {
                    throw new InvalidOperationException("Address must be resolved before creating a property.");
                }

                var property = Domain.Entities.Property.Create(

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
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                createdProperty.Address = address;
                return _propertyMapper.Map(createdProperty);
            }
            catch (NotFoundException)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating property with address");
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
