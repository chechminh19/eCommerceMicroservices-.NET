using eCommerceLibrary.Generic;
using eCommerceLibrary.Response;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.DTOs;
using UserApi.Application.Enums;
using UserApi.Application.Interfaces;
using UserApi.Application.Utils;
using UserApi.Domain.Entities;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Azure.Core;

namespace UserApi.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _config;
        public UserService(IUserRepo userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _config = configuration;
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

            return ResponsesService<int>.Success("User deleted successfully", 200,id);
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

        public async Task<ResponsesService<LoginResponseDTO>> Login(UserRegisterDTO dto)
        {
            try
            {
                var user = await _userRepo.GetUserToLogin(dto.Email);
                if(user == null)
                    return ResponsesService<LoginResponseDTO>.Fail("Email is not correct", 401);
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                        return ResponsesService<LoginResponseDTO>.Fail("Password is not correct", 401);
                if(user.EmailConfirmationToken != null && !user.IsEmailVerified)
                            return ResponsesService<LoginResponseDTO>.Fail("Please confirm via link in your mail", 401);
                var jwt = user.GenerateJsonWebToken(_config["Authentication:Issuer"]!,_config["Authentication:Audience"]!,_config["Authentication:Key"]!,DateTime.UtcNow);
                var response = new LoginResponseDTO { Id = user.Id, Email = user.Email, Token = jwt  };
                                return ResponsesService<LoginResponseDTO>.Success("Login successfully.", 200, response);
            }
            catch (Exception)
            {
                return ResponsesService<LoginResponseDTO>.Fail("Login failed.", 500);
            }
        }       
    }
}
