 
using System.Linq.Expressions;

namespace Common.Interfaces
{
    public interface IGetAllRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> condition);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, object>> include);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, object>> include1, Expression<Func<T, object>> include2);
    }
}
