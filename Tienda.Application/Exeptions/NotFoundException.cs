using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda.Application.Exeptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"Entidad \"{name}\" ({key}) no fue encontrada.")
        {
        }
    }
}