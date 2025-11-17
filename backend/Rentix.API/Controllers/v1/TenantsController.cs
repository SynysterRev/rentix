using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentix.Application.Tenants.Commands.Create;
using Rentix.Application.Tenants.Commands.Delete;
using Rentix.Application.Tenants.Commands.Update;
using Rentix.Application.Tenants.DTOs.Tenants;
using Rentix.Application.Tenants.Queries.List;

namespace Rentix.API.Controllers.v1
{
    public class TenantsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TenantsController> _logger;

        public TenantsController(IMediator mediator, ILogger<TenantsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<TenantDto>>> GetTenants()
        {
            var tenants = await _mediator.Send(new ListTenantsQuery());
            _logger.LogInformation($"Get all tenant");
            return Ok(tenants);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenant(int id, [FromBody] UpdateTenantCommand command)
        {
            if (id != command.TenantId)
            {
                return BadRequest("ID mismatch");
            }
            var updatedTenant = await _mediator.Send(command);
            _logger.LogInformation($"Update tenant with ID {id}");
            return Ok(updatedTenant);
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
