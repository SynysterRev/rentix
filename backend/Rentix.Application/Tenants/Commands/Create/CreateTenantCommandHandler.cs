using MediatR;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Repositories;
using Rentix.Domain.ValueObjects;

namespace Rentix.Application.Tenants.Commands.Create
{
    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, TenantDto>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTenantCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
        {
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var email = Email.Create(request.TenantData.Email);
            var phone = Phone.Create(request.TenantData.PhoneNumber);
            var tenant = Domain.Entities.Tenant.Create(request.TenantData.FirstName, request.TenantData.LastName, email, phone);

            var createdTenant = await _tenantRepository.AddAsync(tenant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return TenantDto.FromEntity(createdTenant);
        }
    }
}
