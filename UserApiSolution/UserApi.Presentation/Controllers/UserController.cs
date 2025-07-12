using eCommerceLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.DTOs;
using UserApi.Application.Interfaces;

namespace UserApi.Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllAsync();
            if (!response.Flag || !response.Data.Any())
            {
                return NotFound(new ApiResponse<IEnumerable<UserDTO>>(404, "No users found", null));
            }
            return Ok(new ApiResponse<IEnumerable<UserDTO>>(200, "Success", response.Data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _userService.GetByIdAsync(id);
            if (!response.Flag)
            {
                return NotFound(new ApiResponse<UserDTO>(404, $"User with ID {id} not found", null));
            }
            return Ok(new ApiResponse<UserDTO>(200, "User found", response.Data));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(400, "Invalid data", ModelState));
            }

            var response = await _userService.RegisterWithoutGoogle(dto);
            return response.Flag
                ? Ok(new ApiResponse<object>(201, response.Message, null))
                : BadRequest(new ApiResponse<object>(400, response.Message, null));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(400, "Invalid data", ModelState));
            }

            var response = await _userService.UpdateAsync(dto);
            return response.Flag
                ? Ok(new ApiResponse<object>(200, response.Message, null))
                : BadRequest(new ApiResponse<object>(400, response.Message, null));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteAsync(id);
            return response.Flag
                ? Ok(new ApiResponse<object>(200, response.Message, null))
                : BadRequest(new ApiResponse<object>(400, response.Message, null));
        }
    }
}
