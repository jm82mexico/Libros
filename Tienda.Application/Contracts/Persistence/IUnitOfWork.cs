using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda.Domain.Common;

namespace Tienda.Application.Contracts.Persistence
{
    public interface IUnitOfWork :IDisposable
    {
        // DECLARAR LOS REPOSITORIOSC CON FUNCIONALIDADES EXTRA A CRUD
        IStreamerRepository StreamerRepository { get; }
        IVideoRepository VideoRepository { get; }

        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel;

        Task<int> Complete();
    }
}