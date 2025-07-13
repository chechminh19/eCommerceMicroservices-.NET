using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;
using eCommerceLibrary.Response;
namespace ProductApi.Presentation.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
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
        public async Task<IActionResult> Create([FromBody]ProductDTO dto)
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
        public async Task<IActionResult> Update([FromBody ]ProductDTO dto, int id)
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
