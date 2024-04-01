using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda.Application.Contracts.Persistence;
using Tienda.Domain;
using Tienda.Infrastructure.Persistence;

namespace Tienda.Infrastructure.Repositories
{
    public class StreamerRepository: RepositoryBase<Streamer>,IStreamerRepository
    {
         public StreamerRepository(StreamerDbContext context) : base(context)
        {
        }
    }
}