using System.Linq.Expressions;

namespace ProyectoIngenieria.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {

        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);


    }
}
