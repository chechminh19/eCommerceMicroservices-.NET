using eCommerceLibrary.Response;
using System.Linq.Expressions;

namespace eCommerceLibrary.Interface
{
    public interface IGeneric<T> where T : class 
    {
        Task<Responses> CreateAsync(T entity);
        Task<Responses> UpdateAsync(T entity);
        Task<Responses> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIdAsync(int id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
