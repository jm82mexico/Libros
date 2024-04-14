using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Tienda.Application.Contracts.Infrastructure;
using Tienda.Application.Contracts.Persistence;
using Tienda.Application.Models;
using Tienda.Domain;

namespace Tienda.Application.Features.Streamers.Commands.CreateStreamer
{
    public class CreateStreamerCommandHandler : IRequestHandler<CreateStreamerCommand, int>
    {
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CreateStreamerCommandHandler> _logger;

        public CreateStreamerCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService, ILogger<CreateStreamerCommandHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
        {
            var streamerEntity = _mapper.Map<Streamer>(request);
            //var newStreamer = await _streamerRepository.AddAsync(streamerEntity);         
            _unitOfWork.StreamerRepository.AddAsync(streamerEntity);

            var result = await _unitOfWork.Complete();

            if (result == 0)
            {
                throw new ApplicationException("There was an error creating the streamer");
            }
              
            
            string message = $"Streamer {streamerEntity.Nombre} was created.";
            _logger.LogInformation(message: message);

            await SendEmail(streamerEntity);

            return streamerEntity.Id;
        }

        private async Task SendEmail(Streamer streamer)
        {
            var email = new Email() 
            {   To = "jmdelan2012@gmail.com",
                Body = $"A new streamer was added: {streamer.Nombre}.",
                Subject = "A new streamer was added"
            };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mailing about streamer {streamer.Nombre} failed due to an error with the mail service: {ex.Message}");
            }
            
        }
    }
}