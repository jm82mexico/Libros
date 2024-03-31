using System.ComponentModel.DataAnnotations.Schema;
using Tienda.Domain.Common;

namespace Tienda.Domain
{
    public class Director : BaseDomainModel
    {
        public Director()
        {
            
        }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }

        //crear una propiedad que concatene el nombre y el apellido
        //pero esta no debe ser mapeada a la base de datos
        [NotMapped]
        public string? NombreCompleto => $"{Nombre} {Apellido}";       

        //agregar una colleccion de videos
        public ICollection<Video>? Videos { get; set; }
    }
}