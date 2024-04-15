using Tienda.Domain;

namespace Tienda.Application.Specifications.Streamers
{
    public class StreamersWithVideosSpecification : BaseSpecification<Streamer>
    {
        public StreamersWithVideosSpecification()
        {
            AddInclude(s => s.Videos!);
        }

        //Se pueden crear especificaciones para cada criterio de busqueda
        //public StreamersWithVideosSpecification(int id) : base(s => s.Id == id)
        //{
        //    AddInclude(s => s.Videos);
        //}
        public StreamersWithVideosSpecification(string url) : base(s => s.Url!.Contains(url))
        {
            AddInclude(s => s.Videos!);
        }
    }
}