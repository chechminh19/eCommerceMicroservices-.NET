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
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _userService.GetAllAsync();
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _userService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _userService.RegisterWithoutGoogle(dto);
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO dto, int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _userService.UpdateAsync(dto, id);
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _userService.DeleteAsync(id);
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }
    }
}
