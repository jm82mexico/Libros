using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Tienda.Application.Contracts.Persistence;
using Tienda.Application.Features.Streamers.Vms;
using Tienda.Application.Specifications.Streamers;
using Tienda.Domain;

namespace Tienda.Application.Features.Streamers.Queries.GetStreamerListByUrl
{
    
    public class GetStreamerListByUrlQueryHandler : IRequestHandler<GetStreamerListByUrlQuery, List<StreamersVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetStreamerListByUrlQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<StreamersVm>> Handle(GetStreamerListByUrlQuery request, CancellationToken cancellationToken)
        {
            //aqui la especificación es la que se encarga de hacer la consulta a la base de datos
            var spec = new StreamersWithVideosSpecification(request.Url!);
            var streamers = await _unitOfWork.Repository<Streamer>().GetAllWithSpec(spec);

            return _mapper.Map<List<StreamersVm>>(streamers);
        }
    }
}