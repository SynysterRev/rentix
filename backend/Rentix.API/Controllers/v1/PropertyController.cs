using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentix.Application.RealEstate.DTOs;
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
        public async Task<ActionResult<List<PropertyDto>>> GetProperties()
        {
            var properties = await _mediator.Send(new ListPropertiesQuery());
            _logger.LogInformation("Get all properties");
            return Ok(properties);
        }
    }
}
