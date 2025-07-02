using eCommerceLibrary.Response;
using System.Linq.Expressions;

namespace eCommerceLibrary.Generic
{
    public interface IGenericRepo<T> where T : class 
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FindByIdAsync(int id);
        Task<T?> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
