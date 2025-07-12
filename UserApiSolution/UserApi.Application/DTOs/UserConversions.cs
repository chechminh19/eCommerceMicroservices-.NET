using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain.Entities;

namespace UserApi.Application.DTOs
{
    public static class UserConversions
    {
        public static User ToEntity(UserCreateDTO dto) => new()
        {
            Email = dto.Email,
            FullName = dto.FullName,
            GoogleId = dto.GoogleId,
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow,
            IsEmailVerified = dto.GoogleId != null
        };

        public static User ToEntity(UserUpdateDTO dto, User existingUser)
        {
            existingUser.FullName = dto.FullName ?? existingUser.FullName;
            existingUser.IsEmailVerified = dto.IsEmailVerified ?? existingUser.IsEmailVerified;
            existingUser.LastLogin = dto.LastLogin ?? existingUser.LastLogin;
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
