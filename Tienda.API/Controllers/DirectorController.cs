using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tienda.Application.Features.Director.Commands.CreateDirector;

namespace Tienda.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DirectorController : ControllerBase
    {
        private IMediator _mediator;

        public DirectorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateDirector")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDirector([FromBody] CreateDirectorCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}