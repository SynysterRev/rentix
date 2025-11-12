using MediatR;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Exceptions;
using Rentix.Domain.Repositories;

namespace Rentix.Application.RealEstate.Commands.Delete
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var deleteDone = await _propertyRepository.DeleteAsync(request.propertyId);

            if (!deleteDone)
            {
                throw new NotFoundException($"Property with ID {request.propertyId} not found.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
