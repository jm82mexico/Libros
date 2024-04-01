using AutoMapper;
using Tienda.Application.Features.Streamers.Commands.CreateStreamer;
using Tienda.Application.Features.Videos.Queries.GetVideosList;
using Tienda.Domain;

namespace Tienda.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Video,VideosVm>();
            CreateMap<CreateStreamerCommand, Streamer>();
        }
    }
}