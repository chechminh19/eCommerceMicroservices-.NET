using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.Enums;

namespace UserApi.Application.DTOs
{

    // DTO update user
    public record UserUpdateDTO(
        [MaxLength(100)] string? FullName,
        UserRole Role
    );

    // DTO return client
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
    public record LoginResponseDTO
    {
        public int Id { get; init; }
        public string Email { get; init; } = default!;
        public string Token { get; init; } = default!;
    }
    public class GoogleUserInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        //public bool VerifiedEmail { get; set; }
    }
    public record LoginResponseGoogleDTO
    {
        public int Id { get; init; }
        public string Email { get; init; }
        //public string GoogleAccessToken { get; init; }
        //public string GoogleIdToken { get; init; }
        public string JwtToken { get; init; }   
    }
}
