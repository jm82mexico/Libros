using Tienda.Domain.Common;

namespace Tienda.Domain
{
    public class VideoActor : BaseDomainModel
    {
        public int VideoId { get; set; }
        //agregar referencia de objetos
        public virtual Video? Video { get; set; }
        
        public int ActorId { get; set; }
        //agregar referencia de objetos
        public virtual Actor? Actor { get; set; }
    }
}