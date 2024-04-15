
using System.Linq.Expressions;


namespace Tienda.Application.Specifications
{
    public interface ISpecification<T>
    {
        //Criterio de la busqueda
        Expression<Func<T, bool>> Criteria { get; }
        //Incluir entidades relacionadas
        List<Expression<Func<T, object>>> Includes { get; }

        //Agregar metodos para ordenar
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        //Paginacion
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnable { get; }
    }
}