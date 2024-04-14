using System.Collections;
using Tienda.Application.Contracts.Persistence;
using Tienda.Domain.Common;
using Tienda.Infrastructure.Persistence;


namespace Tienda.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly StreamerDbContext _dbContext;

        public UnitOfWork(StreamerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<int> Complete()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //log error
                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel
        {
            if(_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if(!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IAsyncRepository<TEntity>)_repositories[type];
        }
    }


}