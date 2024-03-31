using Tienda.Domain.Common;

namespace Tienda.Domain
{
    public class Video : BaseDomainModel
    {
        public Video()
        {
            Actores = new HashSet<Actor>();
        }

        public string?  Nombre { get; set; }
        public int StreamerId { get; set; }
        public virtual Streamer? Streamer { get; set; }

        //crear una propiedad de navegacion a director
        public int? DirectorId { get; set; }
        public virtual Director? Director { get; set; }

        public virtual ICollection<Actor>? Actores { get; set; }

        //crear una propiedad que cree la tabla intermedia entre video y actor
        //esta propiedad no debe ser mapeada a la base de datos        
        public virtual ICollection<VideoActor>? VideoActors { get; set; }
    }
}