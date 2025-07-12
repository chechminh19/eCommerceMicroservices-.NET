using eCommerceLibrary.Generic;
using eCommerceLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.Interfaces;
using UserApi.Domain.Entities;

namespace UserApi.Infrastructure.Repo
{
    public class UserRepo : BaseRepository<User>, IUserRepo
    {
        private readonly UserContext _userContext;
        public UserRepo(UserContext userContext) : base(userContext)
        {
            _userContext = userContext;
        }
        public async Task<User> CreateAsync(User entity)
        {
            try
            {
                var added = _userContext.Users.Add(entity).Entity;
                await _userContext.SaveChangesAsync();
                return added;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(User entity)
        {
            try
            {
                _userContext.Users.Remove(entity);
                var rows = await _userContext.SaveChangesAsync();
                return rows > 0;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _userContext.Users.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            try
            {
                return await _userContext.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<User?> GetByAsync(Expression<Func<User, bool>> predicate)
        {
            try
            {
                return await _userContext.Users.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<User> UpdateAsync(User entity)
        {
            try
            {
                _userContext.Users.Update(entity);
                await _userContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw;
            }
        }

        public async Task<User> GetUserByConfirmationToken(string sToken)
        {
            return await _userContext.Users.SingleOrDefaultAsync(
              u => u.EmailConfirmationToken == sToken
          );
        }
    }
}
