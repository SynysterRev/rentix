using MediatR;
using Microsoft.Extensions.Logging;
using Rentix.Application.Common.Interfaces;
using Rentix.Application.Common.Interfaces.Queries;
using Rentix.Application.Exceptions;
using Rentix.Application.Leases.DTOs;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;
using Rentix.Domain.ValueObjects;

namespace Rentix.Application.Leases.Commands.Create
{
    public class CreateLeaseCommandHandler : IRequestHandler<CreateLeaseCommand, LeaseDto>
    {
        private readonly ILeaseRepository _leaseRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IPropertyQueries _propertyQueries;
        private readonly ITenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<CreateLeaseCommandHandler> _logger;

        public CreateLeaseCommandHandler(ILeaseRepository leaseRepository,
            IDocumentRepository documentRepository,
            IPropertyQueries propertyQueries,
            ITenantRepository tenantRepository,
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService,
            ILogger<CreateLeaseCommandHandler> logger)
        {
            _leaseRepository = leaseRepository;
            _documentRepository = documentRepository;
            _propertyQueries = propertyQueries;
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }
        public async Task<LeaseDto> Handle(CreateLeaseCommand request, CancellationToken cancellationToken)
        {
            if (!await _propertyQueries.ExistsAsync(request.PropertyId))
            {
                throw new NotFoundException($"The property with ID {request.PropertyId} does not exist.");
            }
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var filePath = await _fileStorageService.SaveFileAsync(request.FileStream, request.FileName);

                var newLeaseDocument = Document.Create(LeaseDocumentType.LeaseAgreement,
                    request.FileName,
                    filePath,
                    request.ContentType,
                    request.FileSizeInBytes,
                    request.PropertyId,
                    DocumentEntityType.Lease,
                    null,
                    null);

                var createdDocument = await _documentRepository.AddAsync(newLeaseDocument);

                var newLease = Lease.Create(
                request.StartDate,
                request.EndDate,
                request.RentAmount,
                request.ChargesAmount,
                request.Deposit,
                request.IsActive,
                request.PropertyId,
                createdDocument,
                request.Notes);

                var createdLease = await _leaseRepository.AddAsync(newLease);


                foreach(var tenantDto in request.Tenants)
                {
                    var email = Email.Create(tenantDto.Email);
                    var phone = Phone.Create(tenantDto.PhoneNumber);
                    var tenant = Tenant.Create(tenantDto.FirstName, tenantDto.LastName, email, phone);

                    var createdTenant = await _tenantRepository.AddAsync(tenant);

                    createdLease.Tenants.Add(createdTenant);
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // needed because of the polymorphic relationship
                createdDocument.EntityId = createdLease.Id;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return LeaseDto.FromEntity(createdLease);
            }
            catch (NotFoundException)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lease");
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
