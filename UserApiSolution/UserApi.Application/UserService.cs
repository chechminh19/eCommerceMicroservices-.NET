using eCommerceLibrary.Generic;
using eCommerceLibrary.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.DTOs;
using UserApi.Application.Enums;
using UserApi.Application.Interfaces;
using UserApi.Domain.Entities;

namespace UserApi.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _config;
        private readonly ITransactionRepo _transactionRepo;
        public UserService(IUserRepo userRepo, IConfiguration configuration, ITransactionRepo repo)
        {
            _userRepo = userRepo;
            _config = configuration;
            _transactionRepo = repo;
        }

       

        public async Task<ResponsesService<int>> DeleteAsync(int id)
        {
            var user = await _userRepo.FindByIdAsync(id);
            if (user == null)
                return ResponsesService<int>.Fail("User not found", 404, id);

            bool deleteResult;
            try
            {
                deleteResult = await _userRepo.DeleteAsync(user);
            }
            catch (Exception)
            {
                return ResponsesService<int>.Fail("Internal server error during delete", 500, id);
            }

            if (!deleteResult)
                return ResponsesService<int>.Fail("Failed to delete user due to conflict or constraints", 409, id);

            return ResponsesService<int>.Success("User deleted successfully", id);
        }

        public async Task<ResponsesService<IEnumerable<UserDTO>>> GetAllAsync()
        {
            try
            {
                var users = await _userRepo.GetAllAsync();
                var userDtos = UserConversions.ToDTOs(users);

                return ResponsesService<IEnumerable<UserDTO>>.Success("Users retrieved successfully",200,userDtos);
            }
            catch (Exception)
            {
                return ResponsesService<IEnumerable<UserDTO>>.Fail("Failed to retrieve users",500,Enumerable.Empty<UserDTO>()
                );
            }
        }

        public async Task<ResponsesService<UserDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var user = await _userRepo.FindByIdAsync(id);

                if (user == null)
                    return ResponsesService<UserDTO?>.Fail("User not found", 404, null);

                var userDto = UserConversions.ToDTO(user);
                return ResponsesService<UserDTO?>.Success("User retrieved successfully", 200, userDto);
            }
            catch (Exception)
            {
                return ResponsesService<UserDTO?>.Fail("Failed to retrieve user", 500, null);
            }
        }


        public async Task<ResponsesService<object>> UpdateAsync(UserUpdateDTO dto, int id)
        {
            try
            {
                var user = await _userRepo.FindByIdAsync(id);
                if (user == null)
                    return ResponsesService<object>.Fail("User not found", 404);

                await _userRepo.UpdateAsync(UserConversions.ToEntityUpdate(dto, user));
                return ResponsesService<object>.Success("User updated successfully", 204);
            }
            catch (Exception)
            {
                return ResponsesService<object>.Fail("Failed to update user", 500);
            }
        }       

        public async Task<ResponsesService<object>> RegisterWithoutGoogle(UserRegisterDTO dto)
        {   

            try
            {       
                if (await _userRepo.GetByAsync(u => u.Email == dto.Email) != null)
                    return ResponsesService<object>.Fail("Email already exist.", 400);

                var user = UserConversions.ToEntityRegister(dto);
                await _userRepo.CreateAsync(user);

                if (!await Utils.EmailUtils.SendConfirmationEmail(user.Email, user.EmailConfirmationToken, _config))
                {
                    return ResponsesService<object>.Fail("Failed to send confirmation email.", 500);
                }
                                 
                return ResponsesService<object>.Success("User created successfully.", 201);
            }
            catch (Exception)
            {
                return ResponsesService<object>.Fail("An unexpected error occurred.", 500);
            }
        }
    }
}
