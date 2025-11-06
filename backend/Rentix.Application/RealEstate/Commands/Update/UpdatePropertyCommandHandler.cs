using MediatR;
using Rentix.Application.Exceptions;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Domain.Repositories;
using System.Threading.Tasks;

namespace Rentix.Application.RealEstate.Commands.Update
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, PropertyDetailDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyDetailDto> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(request.propertyId);
            if (property == null)
            {
                throw new NotFoundException($"Property with ID {request.propertyId} not found");
            }

            if (request.Address != null && property.Address != null)
            {
                property.Address.UpdateDetails(
                    request.Address.Street,
                    request.Address.PostalCode,
                    request.Address.City,
                    request.Address.Country,
                    request.Address.Complement);
            }

            property.UpdateDetails(
                request.Name, 
                request.Deposit, 
                request.RentNoCharges, 
                request.RentCharges, 
                request.Status,
                request.Surface, 
                request.NumberRooms);
            await _unitOfWork.SaveChangesAsync();

            return PropertyDetailDto.FromEntity(property);
        }
    }
}
