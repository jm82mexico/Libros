using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Tienda.Application.Contracts.Persistence;
using Tienda.Application.Exeptions;
using Tienda.Domain;

namespace Tienda.Application.Features.Streamers.Commands.DeleteStreamer
{
    public class DeleteStreamerCommandHandler : IRequestHandler<DeleteStreamerCommand>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteStreamerCommandHandler> _logger;

        public DeleteStreamerCommandHandler(IMapper mapper, IStreamerRepository streamerRepository, ILogger<DeleteStreamerCommandHandler> logger)
        {
            _mapper = mapper;
            _streamerRepository = streamerRepository;
            _logger = logger;
        }
        public async Task Handle(DeleteStreamerCommand request, CancellationToken cancellationToken)
        {
            var streamerToDelete = await _streamerRepository.GetByIdAsync(request.Id);
           
            if (streamerToDelete == null)
            {
                _logger.LogError($"{request.Id} streamer no existe en el sistema");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

            await _streamerRepository.DeleteAsync(streamerToDelete);          

            _logger.LogInformation($"El {request.Id} streamer fue eliminado con exito");
        }
    }

}