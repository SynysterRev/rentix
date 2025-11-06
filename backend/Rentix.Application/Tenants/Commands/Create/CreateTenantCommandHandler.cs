using MediatR;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var tenant = Domain.Entities.Tenant.Create(request.FirstName, request.LastName, request.Email, request.Phone);

            var createdTenant = await _tenantRepository.AddAsync(tenant);
            await _unitOfWork.SaveChangesAsync();

            return TenantDto.FromEntity(createdTenant);
        }
    }
}
