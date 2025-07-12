using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.Enums;

namespace UserApi.Application.DTOs
{
    // DTO khi tạo user mới
    public record UserCreateDTO(
        [Required][EmailAddress] string Email,
        [MaxLength(255)] string? GoogleId,
        [MaxLength(100)] string? FullName,
        UserRole Role = UserRole.User
    );

    // DTO khi cập nhật user
    public record UserUpdateDTO(
        [Required] int Id,
        [MaxLength(100)] string? FullName,
        UserRole Role,
        bool? IsEmailVerified,
        DateTime? LastLogin
    );

    // DTO khi trả về client
    public record UserDTO(
        int Id,
        string Email,
        string? FullName,
        DateTime CreatedAt,
        DateTime? LastLogin,
        bool? IsEmailVerified,
        UserRole Role
    );
    public record UserRegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be 6-20 characters")]
        public string Password { get; set; }
    }
}
