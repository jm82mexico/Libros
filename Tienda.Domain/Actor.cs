using System.ComponentModel.DataAnnotations.Schema;
using Tienda.Domain.Common;

namespace Tienda.Domain
{
    public class Actor : BaseDomainModel
    {
        public Actor() { 
            Videos = new HashSet<Video>();
        }

        //crear una propiedad que concatene el nombre y el apellido
        //pero esta no debe ser mapeada a la base de datos
        [NotMapped]
        public string? NombreCompleto => $"{Nombre} {Apellido}";
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public virtual ICollection<Video> Videos { get; set; }

        //crea una propiedad que cree la tabla intermedia entre video y actor

        public virtual ICollection<VideoActor>? VideoActors { get; set; }
    }
}