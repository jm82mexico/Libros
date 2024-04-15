using System.Linq.Expressions;

namespace Tienda.Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        //Implementacion de la interfaz ISpecification
        //Criterio de la busqueda
        
        public Expression<Func<T, bool>> Criteria { get; }        
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();


        public bool IsPagingEnabled => throw new NotImplementedException();

        //Constructor
        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        //Propiedades para ordenar
        
        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        //metodos para ordenar
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }        

        //Propiedades  para paginacion

        public int Take { get; private set; }
        public int Skip { get; private set; }        
        public bool IsPagingEnable { get; private set; }

        //metodos para paginacion
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnable = true;
        }

        //Metodos para incluir entidades relacionadas
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}