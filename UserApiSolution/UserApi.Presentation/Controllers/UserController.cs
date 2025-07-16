using eCommerceLibrary.Response;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.DTOs;
using UserApi.Application.Interfaces;
using Google.Apis.Auth;
using UserApi.Application;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain.Entities;

namespace UserApi.Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoogleAuthor _googleAuthor;
        public UserController(IUserService userService, IGoogleAuthor googleAuthor)
        {
            _userService = userService;
            _googleAuthor = googleAuthor;    
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
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
            return StatusCode(response.StatusCode, new ApiResponse<IEnumerable<UserDTO>>(response.Flag, response.StatusCode, response.Message, response.Data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
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
            return StatusCode(response.StatusCode, new ApiResponse<UserDTO>(response.Flag, response.StatusCode, response.Message, response.Data));
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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _userService.Login(dto);
            return StatusCode(response.StatusCode,
                new ApiResponse<LoginResponseDTO>(response.Flag, response.StatusCode, response.Message, response.Data));
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin() => Ok(_googleAuthor.GetAuthorizationUrl());


        [HttpGet("authorize/callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest(new ApiResponse<object>(
                    false, 400, "Missing Google authorization code", null));
            }

            var result = await _googleAuthor.ExchangeCodeForToken(code);

            return StatusCode(result.StatusCode,
                new ApiResponse<LoginResponseGoogleDTO>(result.Flag, result.StatusCode, result.Message, result.Data));
        }

        [HttpPost("google-validate")]
        public async Task<IActionResult> ValidateGoogleToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request?.Token))
                return BadRequest("Token is required");

            try
            {
                var user = await _googleAuthor.ValidCodeForToken(request.Token);

                return Ok(new
                {
                    UserId = user.Id,
                    user.Email,
                    user.FullName,
                    user.GoogleId
                });
            }
            catch (InvalidJwtException ex)
            {
                return Unauthorized(new { Error = "Invalid Google token", Details = ex.Message });
            }
        }
    }
    public class TokenRequest
    {
        public string Token { get; set; }
    }

}
