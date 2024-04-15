using Tienda.Application.Features.Videos.Queries.GetVideosList;

namespace Tienda.Application.Features.Streamers.Vms
{
    public class StreamersVm
    {
        public string? Nombre { get; set; }
        public string? Url { get; set; }
        public virtual ICollection<VideosVm>? Videos { get; set; }
    }
}