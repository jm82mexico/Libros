
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Tienda.Application.Contracts.Persistence;
using Tienda.Application.Exeptions;
using Tienda.Domain;

namespace Tienda.Application.Features.Streamers.Commands.UpdateStreamer
{
    public class UpdateStreamerCommandHandler : IRequestHandler<UpdateStreamerCommand>
    {
        private readonly IStreamerRepository _streamerRepository;     
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateStreamerCommandHandler> _logger;

        public UpdateStreamerCommandHandler(IMapper mapper, IStreamerRepository streamerRepository, ILogger<UpdateStreamerCommandHandler> logger)
        {
            _mapper = mapper;
            _streamerRepository = streamerRepository;
            _logger = logger;
        }
        public async Task Handle(UpdateStreamerCommand request, CancellationToken cancellationToken)
        {
            var streamerToUpdate = await _streamerRepository.GetByIdAsync(request.Id);
            

            if (streamerToUpdate == null)
            {
                _logger.LogError($"No se encontro el streamer id {request.Id}");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

            _mapper.Map(request, streamerToUpdate, typeof(UpdateStreamerCommand), typeof(Streamer));

            await _streamerRepository.UpdateAsync(streamerToUpdate);          

            _logger.LogInformation($"La operacion fue exitosa actualizando el streamer {request.Id}");
        }
    }
}