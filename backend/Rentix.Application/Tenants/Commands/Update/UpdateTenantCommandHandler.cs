using MediatR;
using Rentix.Application.Exceptions;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Repositories;
using Rentix.Domain.ValueObjects;

namespace Rentix.Application.Tenants.Commands.Update
{
    public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, TenantDto>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTenantCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
        {
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TenantDto> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetTenantByIdAsync(request.TenantId);
            if (tenant == null)
            {
                throw new NotFoundException($"Tenant with ID {request.TenantId} not found.");
            }

            tenant.UpdateDetails(
                request.FirstName,
                request.LastName,
                request.Email != null ? Email.Create(request.Email) : null,
                request.Phone != null ? Phone.Create(request.Phone) : null
            );

            await _unitOfWork.SaveChangesAsync();

            return TenantDto.FromEntity(tenant);
        }
    }
}
