using eCommerceLibrary.Response;
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
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ResponsesServiceDTO<UserCreateDTO>> CreateAsync(UserCreateDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email))
                    return new ResponsesServiceDTO<UserCreateDTO>(false, "Email is required", null);

                if (await _userRepo.GetByAsync(u => u.Email == dto.Email) != null)
                    return new ResponsesServiceDTO<UserCreateDTO>(false, "Email already exists", null);

                var user = UserConversions.ToEntity(dto);
                await _userRepo.CreateAsync(user);

                return new ResponsesServiceDTO<UserCreateDTO>(true, "User created successfully", dto);
            }
            catch (Exception)
            {
                return new ResponsesServiceDTO<UserCreateDTO>(false, "Failed to create user", null);
            }
        }

        public async Task<ResponsesService> DeleteAsync(int id)
        {
            try
            {
                var user = await _userRepo.FindByIdAsync(id);
                if (user == null)
                    return new ResponsesService(false, "User not found");

                if (!await _userRepo.DeleteAsync(user))
                    return new ResponsesService(false, "Failed to delete user");

                return new ResponsesService(true, "User deleted successfully");
            }
            catch (Exception)
            {
                return new ResponsesService(false, "Failed to delete user");
            }
        }

        public async Task<ResponsesServiceDTO<IEnumerable<UserDTO>>> GetAllAsync()
        {
            try
            {
                var users = await _userRepo.GetAllAsync();
                return new ResponsesServiceDTO<IEnumerable<UserDTO>>(
                    true,
                    "Users retrieved successfully",
                    UserConversions.ToDTOs(users)
                );
            }
            catch (Exception)
            {
                return new ResponsesServiceDTO<IEnumerable<UserDTO>>(
                    false,
                    "Failed to retrieve users",
                    Enumerable.Empty<UserDTO>()
                );
            }
        }

        public async Task<ResponsesServiceDTO<UserDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var user = await _userRepo.FindByIdAsync(id);
                return user == null
                    ? new ResponsesServiceDTO<UserDTO?>(false, "User not found", null)
                    : new ResponsesServiceDTO<UserDTO?>(true, "User retrieved successfully", UserConversions.ToDTO(user));
            }
            catch (Exception)
            {
                return new ResponsesServiceDTO<UserDTO?>(false, "Failed to retrieve user", null);
            }
        }

        public async Task<ResponsesServiceDTO<UserUpdateDTO>> UpdateAsync(UserUpdateDTO dto)
        {
            try
            {
                var user = await _userRepo.FindByIdAsync(dto.Id);
                if (user == null)
                    return new ResponsesServiceDTO<UserUpdateDTO>(false, "User not found", null);

                var updatedUser = await _userRepo.UpdateAsync(UserConversions.ToEntity(dto, user));
                return new ResponsesServiceDTO<UserUpdateDTO>(true, "User updated successfully", dto);
            }
            catch (Exception)
            {
                return new ResponsesServiceDTO<UserUpdateDTO>(false, "Failed to update user", null);
            }
        }

        public async Task<ResponsesServiceDTO<UserDTO>> HandleGoogleUserAsync(string googleId, string email, string name)
        {
            try
            {
                var existingUser = await _userRepo.GetByAsync(u => u.GoogleId == googleId || u.Email == email);

                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        Email = email,
                        GoogleId = googleId,
                        FullName = name,
                        IsEmailVerified = true,
                        CreatedAt = DateTime.UtcNow,
                        Role = UserRole.User
                    };
                    existingUser = await _userRepo.CreateAsync(newUser);
                }
                else if (string.IsNullOrEmpty(existingUser.GoogleId))
                {
                    existingUser.GoogleId = googleId;
                    existingUser.IsEmailVerified = true;
                    existingUser = await _userRepo.UpdateAsync(existingUser);
                }

                return new ResponsesServiceDTO<UserDTO>(
                    true,
                    "Google authentication successful",
                    UserConversions.ToDTO(existingUser)
                );
            }
            catch (Exception)
            {
                return new ResponsesServiceDTO<UserDTO>(
                    false,
                    "Failed to authenticate with Google",
                    null
                );
            }
        }
    }
}
