using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentix.Application.Tenants.Commands.Create;
using Rentix.Application.Tenants.Commands.Delete;

namespace Rentix.API.Controllers.v1
{
    public class TenantController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TenantController> _logger;

        public TenantController(IMediator mediator, ILogger<TenantController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTenant(int id)
        {
            // Implementation for getting a tenant by ID would go here
            _logger.LogInformation($"Get tenant with ID {id}");
            return Ok(); // Placeholder
        }

        [HttpPost]
        public async Task<ActionResult> CreateTenant(CreateTenantCommand command)
        {
            var tenant = await _mediator.Send(command);
            _logger.LogInformation($"Create tenant with ID {tenant.Id}");
            return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenant);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTenant(int id)
        {
            await _mediator.Send(new DeleteTenantCommand(id));
            _logger.LogInformation($"Delete tenant with ID {id}");
            return NoContent();
        }
    }
}
