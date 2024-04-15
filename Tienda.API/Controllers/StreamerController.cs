using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tienda.Application.Features.Streamers.Commands.CreateStreamer;
using Tienda.Application.Features.Streamers.Commands.DeleteStreamer;
using Tienda.Application.Features.Streamers.Commands.UpdateStreamer;
using Tienda.Application.Features.Streamers.Queries.GetStreamerListByUsername;
using Tienda.Application.Features.Streamers.Vms;


namespace Tienda.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StreamerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StreamerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name ="CreateStreamer")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateStreamer([FromBody] CreateStreamerCommand command)
        {
            return await _mediator.Send(command);
            
        }

        [HttpPut(Name ="UpdateStreamer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateStreamer([FromBody] UpdateStreamerCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}",Name ="DeleteStreamer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteStreamer(int id)
        {
            var command = new DeleteStreamerCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("ByUsername/{username}",Name = "GetStreamersByUsername")]
        [ProducesResponseType(typeof(List<StreamersVm>), (int) HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<StreamersVm>>> GetStreamersByUsername(string username)
        {
            var query = new GetStreamerListQuery(username);
            var streamers = await _mediator.Send(query);
            return Ok(streamers);
        }

    }
}