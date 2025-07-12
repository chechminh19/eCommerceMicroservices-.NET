using eCommerceLibrary.Generic;
using eCommerceLibrary.Response;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.Interfaces
{
    public interface IProductRepo
    {
        //Add more method if need
        public Task<Product> CreateAsync(Product entity);
        public Task<bool> DeleteAsync(Product entity);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<Product?> FindByIdAsync(int id);
        public Task<Product?> GetByAsync(Expression<Func<Product, bool>> predicate);
        public Task<Product> UpdateAsync(Product entity);

    }
}
        