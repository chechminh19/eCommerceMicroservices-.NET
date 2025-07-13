using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.Enums;
using UserApi.Domain.Entities;
using UserApi.Domain.Enums;

namespace UserApi.Application.DTOs
{
    public static class UserConversions
    {      
        public static User ToEntityRegister(UserRegisterDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new User
            {
                Email = dto.Email.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = UserActive.Pending,
                EmailConfirmationToken = Guid.NewGuid().ToString(),
                EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(2),
                IsEmailVerified = false,
                LastLogin = null,
                GoogleId = null,
                PasswordResetToken = null,
                PasswordResetTokenExpiry = null,
                RefreshToken = null,
                RefreshTokenExpiry = null,
                Role = UserRole.User
            };
        }
        public static User ToEntityUpdate(UserUpdateDTO dto, User existingUser)
        {
            existingUser.FullName = dto.FullName ?? existingUser.FullName;
            return existingUser;
        }

        public static UserDTO ToDTO(User entity) => new(
            entity.Id,
            entity.Email,
            entity.FullName,
            entity.CreatedAt,
            entity.LastLogin,
            entity.IsEmailVerified,
            entity.Role
        );

        public static IEnumerable<UserDTO> ToDTOs(IEnumerable<User> users)
            => users.Select(ToDTO);
    }
}
