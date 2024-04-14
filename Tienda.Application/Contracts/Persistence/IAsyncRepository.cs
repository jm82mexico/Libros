using System.Linq.Expressions;
using Tienda.Domain.Common;

namespace Tienda.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : BaseDomainModel
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                            string? includeString = null,
                                             bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
                                    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                    List<Expression<Func<T, object>>>? includes = null,
                                    bool disableTracking = true);

        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        //Metodos de unit of work
        //Metodos para uso de unit of work
        void AddEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);
        
        //Metodos de specification
        /* Task<T> GetByIdWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);//Se corrige para que devuelva una lista por la implementacion del paginado
        Task<int> CountAsync(ISpecification<T> spec); */
    }
}