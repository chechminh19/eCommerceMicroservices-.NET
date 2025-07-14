using eCommerceLibrary.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain.Entities;

namespace UserApi.Application.Interfaces
{
    public interface IUserRepo
    {
        public Task<User> CreateAsync(User entity);
        public Task<bool> DeleteAsync(User entity);
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User?> FindByIdAsync(int id);
        public Task<User?> GetByAsync(Expression<Func<User, bool>> predicate);
        public Task<User> UpdateAsync(User entity);
        Task<User> GetUserByConfirmationToken(string sToken);
        Task<User> GetUserToLogin(string email);
    }
}
