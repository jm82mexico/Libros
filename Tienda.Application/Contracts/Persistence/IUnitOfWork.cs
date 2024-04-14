using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda.Domain.Common;

namespace Tienda.Application.Contracts.Persistence
{
    public interface IUnitOfWork :IDisposable
    {
        
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel;

        Task<int> Complete();
    }
}