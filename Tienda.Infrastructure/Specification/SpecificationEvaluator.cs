using Microsoft.EntityFrameworkCore;
using Tienda.Application.Specifications;
using Tienda.Domain.Common;

namespace Tienda.Infrastructure.Specification
{
    //Esta es la clase que se conecta con unitOfWork y generic repository
    public class SpecificationEvaluator<T> where T : BaseDomainModel
    {
        //Metodo para aplicar los criterios de busqueda
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            //Aplicar los criterios de busqueda
            var query = inputQuery;
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            //Aplicar las entidades relacionadas
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            //se agrega la logica para ordenar
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            //se agrega la logica para paginar
            if (specification.IsPagingEnable)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }
    }
}