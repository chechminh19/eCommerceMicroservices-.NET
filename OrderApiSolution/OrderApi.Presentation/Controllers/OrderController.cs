using eCommerceLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.Enums;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService orderService)
        {
            _service = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _service.GetAllAsync();
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _service.GetByIdAsync(id);
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _service.CreateAsync(dto);
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] OrderUpdateDTO dto, int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _service.UpdateAsync(dto, id);
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(kvp => kvp.Key,
                                      kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<object>(false, 400, "Invalid data", null, errors));
            }

            var response = await _service.DeleteAsync(id);
            return StatusCode(response.StatusCode, new ApiResponse<object>(response.Flag, response.StatusCode, response.Message, null));
        }   

    }
}
