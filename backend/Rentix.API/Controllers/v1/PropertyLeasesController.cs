using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentix.API.Models;
using Rentix.Application.Leases.Commands.Create;
using Rentix.Application.Leases.DTOs;

namespace Rentix.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/properties/{propertyId}/leases")]
    public class PropertyLeasesController : BaseController
    {
        private readonly ILogger<PropertyLeasesController> _logger;
        private readonly IMediator _mediator;

        public PropertyLeasesController(ILogger<PropertyLeasesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<LeaseDto>> GetLease(int propertyId)
        {
            //var leases = await _mediator.Send(new GetLeasesByPropertyIdQuery { PropertyId = propertyId });
            //_logger.LogInformation($"Retrieved {leases.Count} leases for property ID {propertyId}.");
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<LeaseDto>> CreateLease(int propertyId, [FromForm] LeaseRequestDto leaseRequest)
        {
            if (leaseRequest.LeaseDocument == null || leaseRequest.LeaseDocument.Length == 0)
            {
                return BadRequest("Lease document is required.");
            }

            var fileName = leaseRequest.LeaseDocument.FileName;
            var fileSize = leaseRequest.LeaseDocument.Length;
            var contentType = leaseRequest.LeaseDocument.ContentType;

            var leaseCommand = new CreateLeaseCommand
            {
                PropertyId = propertyId,
                StartDate = leaseRequest.StartDate,
                EndDate = leaseRequest.EndDate,
                RentAmount = leaseRequest.RentAmount,
                ChargesAmount = leaseRequest.ChargesAmount,
                Deposit = leaseRequest.Deposit,
                IsActive = leaseRequest.IsActive,
                Notes = leaseRequest.Notes,
                ContentType = contentType,
                FileStream = leaseRequest.LeaseDocument.OpenReadStream(),
                FileSizeInBytes = fileSize,
                FileName = fileName,
                Tenants = leaseRequest.Tenants
            };
            var leaseDto = await _mediator.Send(leaseCommand);

            _logger.LogInformation($"Lease created for property ID {propertyId}.");

            return CreatedAtAction(nameof(GetLease), new { propertyId, leaseId = leaseDto.Id }, leaseDto);
        }
    }
}
