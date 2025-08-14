using eCommerceLibrary.Generic;
using eCommerceLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Enums;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepo :BaseRepository<Order>, IOrderRepo
    {
        private readonly OrderContext _context;
        public OrderRepo(OrderContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Order> CreateAsync(Order entity)
        {
            try
            {
                var added = _context.Orders.Add(entity).Entity;
                await _context.SaveChangesAsync();
                return added;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Order entity)
        {
            try
            {
                _context.Orders.Remove(entity);
                var rows = await _context.SaveChangesAsync();
                return rows > 0;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                return await _context.Orders.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<Order?> FindByIdAsync(int id)
        {
            try
            {
                return await _context.Orders.Include(o => o.OrderDetails)
                                        .FirstOrDefaultAsync(o => o.Id == id);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<Order?> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                return await _context.Orders.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            try
            {
                _context.Orders.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<Order?> FindCartByUserIdAsync(int userId)
        {
            return await _context.Orders
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == (byte)OrderEnums.Processing);
        }
    }
}
