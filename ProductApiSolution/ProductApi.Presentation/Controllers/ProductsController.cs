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
            var list = await _service.GetAllAsync();
            if (!list.Any())
            {
                return NotFound(new ApiResponse<IEnumerable<ProductDTO>>(404, "No products found", null));
            }
            return Ok(new ApiResponse<IEnumerable<ProductDTO>>(200, "Success", list));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ApiResponse<ProductDTO>(404, $"Product with ID {id} not found", null));
            }
            return Ok(new ApiResponse<ProductDTO>(200, "Product found", product));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductDTO dto)
        {
            var res = await _service.CreateAsync(dto);
            return res.Flag ? Ok(new ApiResponse<object>(200, res.Message, null))
                               : BadRequest(new ApiResponse<object>(400, res.Message, null));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductDTO dto)
        {
            var res = await _service.UpdateAsync(dto);
            return res.Flag ? Ok(new ApiResponse<object>(200, res.Message, null))
                              : BadRequest(new ApiResponse<object>(400, res.Message, null));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _service.DeleteAsync(id);
            return res.Flag ? Ok(new ApiResponse<object>(200, res.Message, null))
                              : BadRequest(new ApiResponse<object>(400, res.Message, null));
        }
    }
}
