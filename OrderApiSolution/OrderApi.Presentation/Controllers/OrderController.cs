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
            var response = await _service.GetAllAsync();
            if (!response.Flag || !response.Data.Any())
            {
                return NotFound(new ApiResponse<IEnumerable<OrderDTO>>(404, "No orders found", null));
            }
            return Ok(new ApiResponse<IEnumerable<OrderDTO>>(200, "Success", response.Data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            if (!response.Flag)
            {
                return NotFound(new ApiResponse<OrderDTO>(404, $"Order with ID {id} not found", null));
            }
            return Ok(new ApiResponse<OrderDTO>(200, "Order found", response.Data));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(400, "Invalid data", ModelState));
            }

            var response = await _service.CreateAsync(dto);
            return response.Flag
                ? Ok(new ApiResponse<object>(200, response.Message, null))
                : BadRequest(new ApiResponse<object>(400, response.Message, null));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OrderUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(400, "Invalid data", ModelState));
            }

            var response = await _service.UpdateAsync(dto);
            return response.Flag
                ? Ok(new ApiResponse<object>(200, response.Message, null))
                : BadRequest(new ApiResponse<object>(400, response.Message, null));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            return response.Flag
                ? Ok(new ApiResponse<object>(200, response.Message, null))
                : BadRequest(new ApiResponse<object>(400, response.Message, null));
        }   

    }
}
