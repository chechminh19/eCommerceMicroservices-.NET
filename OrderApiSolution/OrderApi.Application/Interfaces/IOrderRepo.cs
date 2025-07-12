using eCommerceLibrary.Generic;
using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Interfaces
{
    public interface IOrderRepo
    {
        //add more method
        public Task<Order> CreateAsync(Order entity);
        public Task<bool> DeleteAsync(Order entity);
        public Task<IEnumerable<Order>> GetAllAsync();
        public Task<Order?> FindByIdAsync(int id);
        public Task<Order?> GetByAsync(Expression<Func<Order, bool>> predicate);
        public Task<Order> UpdateAsync(Order entity);

    }
}
