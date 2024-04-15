using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Tienda.Application.Contracts.Persistence;
using Tienda.Application.Features.Streamers.Vms;
using Tienda.Domain;

namespace Tienda.Application.Features.Streamers.Queries.GetStreamerListByUsername
{
    public class GetStreamerListByUsername : IRequestHandler<GetStreamerListQuery, List<StreamersVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStreamerListByUsername(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<StreamersVm>> Handle(GetStreamerListQuery request, CancellationToken cancellationToken)
        {
            //Se crea una lista de expresiones para incluir las entidades relacionadas
            var includes = new List<Expression<Func<Streamer, object>>>();
            //Se incluye la entidad Videos
            includes.Add(x => x.Videos);

            //Se obtiene la lista de streamers que coincidan con el nombre de usuario
            var streamersList = await _unitOfWork.Repository<Streamer>().GetAsync(
                //Se configura el criterio de búsqueda
                b => b.CreatedBy   == request.Username,
                //Se configura el criterio de ordenamiento
                b=>b.OrderBy(x=>x.CreatedBy),
                //Se incluyen las entidades relacionadas
                includes,
                true
            );
            return _mapper.Map<List<StreamersVm>>(streamersList);
        }
    }
}
