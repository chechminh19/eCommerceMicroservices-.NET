using eCommerceLibrary.Generic;
using eCommerceLibrary.Response;
using Google.Apis.Auth.OAuth2;
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
        Task<ResponsesService<object>> RegisterWithoutGoogle(UserRegisterDTO dto);
        Task<ResponsesService<LoginResponseDTO>> Login(UserRegisterDTO dto);
        Task<ResponsesService<object>> UpdateAsync(UserUpdateDTO dto, int id);
        Task<ResponsesService<int>> DeleteAsync(int id);
        Task<ResponsesService<UserDTO?>> GetByIdAsync(int id);
        Task<ResponsesService<IEnumerable<UserDTO>>> GetAllAsync();
        
    }
}
