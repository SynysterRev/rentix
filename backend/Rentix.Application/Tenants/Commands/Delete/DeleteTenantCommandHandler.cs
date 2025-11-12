using MediatR;
using Rentix.Application.Exceptions;
using Rentix.Domain.Repositories;

namespace Rentix.Application.Tenants.Commands.Delete
{
    public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteTenantCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
        {
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            var deleteDone = await _tenantRepository.DeleteAsync(request.TenantId);

            if (!deleteDone)
            {
                throw new NotFoundException($"Tenant with ID {request.TenantId} not found.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
