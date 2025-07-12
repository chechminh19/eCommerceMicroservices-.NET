using eCommerceLibrary.Generic;
using eCommerceLibrary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.DTOs;

namespace UserApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponsesServiceDTO<UserCreateDTO>> CreateAsync(UserCreateDTO dto);
        Task<ResponsesService> RegisterWithoutGoogle(UserRegisterDTO dto);
        Task<ResponsesServiceDTO<UserUpdateDTO>> UpdateAsync(UserUpdateDTO dto);
        Task<ResponsesService> DeleteAsync(int id);
        Task<ResponsesServiceDTO<UserDTO?>> GetByIdAsync(int id);
        Task<ResponsesServiceDTO<IEnumerable<UserDTO>>> GetAllAsync();
    }
}
