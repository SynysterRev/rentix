using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentix.Application.RealEstate.Commands.Create.Property;
using Rentix.Application.RealEstate.Commands.Delete;
using Rentix.Application.RealEstate.Commands.Update;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.Queries.Detail;
using Rentix.Application.RealEstate.Queries.List;

namespace Rentix.API.Controllers.v1
{
    public class PropertiesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PropertiesController> _logger;

        public PropertiesController(IMediator mediator, ILogger<PropertiesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<PropertyListDto>>> GetProperties()
        {
            var properties = await _mediator.Send(new ListPropertiesQuery());
            _logger.LogInformation("Get all properties");
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDetailDto>> GetPropertyDetail(int id)
        {
            var property = await _mediator.Send(new DetailPropertyQuery(id));
            _logger.LogInformation($"Get property with ID {id}");
            return Ok(property);
        }

        [HttpPost]
        public async Task<ActionResult<PropertyDetailDto>> CreateProperty([FromBody] CreatePropertyCommand command)
        {
            var newProperty = await _mediator.Send(command);
            _logger.LogInformation($"Create property with ID {newProperty.Id}");
            return CreatedAtAction(nameof(GetPropertyDetail), new { id = newProperty.Id }, newProperty);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            await _mediator.Send(new DeletePropertyCommand(id));
            _logger.LogInformation($"Delete property with ID {id}");
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PropertyDetailDto>> UpdateProperty(int id, [FromBody] UpdatePropertyCommand command)
        {
            if (id != command.propertyId)
            {
                return BadRequest("Property ID mismatch");
            }
            var updatedProperty = await _mediator.Send(command);
            _logger.LogInformation($"Update property with ID {id}");
            return Ok(updatedProperty);
        }
    }
}
