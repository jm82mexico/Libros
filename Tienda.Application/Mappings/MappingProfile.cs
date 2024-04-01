using AutoMapper;
using Tienda.Application.Features.Videos.Queries.GetVideosList;
using Tienda.Domain;

namespace Tienda.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Video,VideosVm>();
        }
    }
}