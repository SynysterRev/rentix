using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentix.API.Models;
using Rentix.Application.Leases.Commands.Create;

namespace Rentix.API.Controllers.v1
{
    public class LeasesController : BaseController
    {
        private readonly ILogger<LeasesController> _logger;
        private readonly IMediator _mediator;

        public LeasesController(ILogger<LeasesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        //[HttpPost("preview")]
        //public async Task<IResult> PreviewLease(CreateLeaseCommand leaseCommand)
        //{
        //    var previewLease = await _mediator.Send(leaseCommand);

        //    return Results.File(previewLease, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "test.doc");
        //}
    }
}
