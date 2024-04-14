
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Tienda.Application.Contracts.Persistence;
using Tienda.Domain;

namespace Tienda.Application.Features.Director.Commands.CreateDirector
{
    public class CreateDirectorCommandHandler : IRequestHandler<CreateDirectorCommand, int>
    {
        private readonly ILogger<CreateDirectorCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDirectorCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, ILogger<CreateDirectorCommandHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<int> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
        {
            var directorEntity = _mapper.Map<Tienda.Domain.Director>(request);
            _unitOfWork.Repository<Tienda.Domain.Director>().AddEntity(directorEntity);

            var result = await _unitOfWork.Complete();

            if(result <= 0)
            {
                _logger.LogError($"Director {directorEntity.Nombre} {directorEntity.Apellido} failed to be created.");
                throw new Exception("No se pudo crear el director.");
                
            }

            return directorEntity.Id;
        }
    }
}