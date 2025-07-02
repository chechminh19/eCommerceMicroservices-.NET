using eCommerceLibrary.Generic;
using eCommerceLibrary.Logs;
using eCommerceLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepo : BaseRepository<Product>, IProductRepo
    {
        private readonly ProductContext _context;

        public ProductRepo(ProductContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            try
            {
                var added = _context.Products.Add(entity).Entity;
                await _context.SaveChangesAsync();
                return added;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Product entity)
        {
            try
            {
                _context.Products.Remove(entity);
                var rows = await _context.SaveChangesAsync();
                return rows > 0;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                return await _context.Products.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<Product?> FindByIdAsync(int id)
        {
            try
            {
                return await _context.Products.FindAsync(id);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<Product?> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                return await _context.Products.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            try
            {
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }
    }
}
