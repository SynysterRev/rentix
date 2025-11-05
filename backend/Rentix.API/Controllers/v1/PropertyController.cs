using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentix.Application.RealEstate.Commands.Create;
using Rentix.Application.RealEstate.DTOs.Properties;
using Rentix.Application.RealEstate.Queries.Detail;
using Rentix.Application.RealEstate.Queries.List;

namespace Rentix.API.Controllers.v1
{
    public class PropertyController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(IMediator mediator, ILogger<PropertyController> logger)
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
            return CreatedAtAction(nameof(GetPropertyDetail), new { id = newProperty.Id }, newProperty);
        }
    }
}
