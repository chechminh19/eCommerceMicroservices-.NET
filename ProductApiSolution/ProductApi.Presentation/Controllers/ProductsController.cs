using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;
using eCommerceLibrary.Response;
namespace ProductApi.Presentation.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProductsController(IProduct Iproduct) : ControllerBase
    {
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await Iproduct.GetAllAsync();
            if(products == null) {
                return NotFound("No Found");
            }
            var list = ProductConversion.FromEntities(products);
            return Ok(list);
        }
        [HttpGet("product/{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await Iproduct.FindByIdAsync(id);
            if(product == null)
            {
                return NotFound($"Product {product.Id} requested not found");
            }
            var _product = ProductConversion.FromEntityNew(product);
            return Ok(_product);
        }
        [HttpPost("product")]
        public async Task<ActionResult<eCommerceLibrary.Response.Responses>> CreateProduct(ProductDTO product)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntity = ProductConversion.ToEntity(product);
            var response = await Iproduct.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
        [HttpPut("product")]
        public async Task<ActionResult<eCommerceLibrary.Response.Responses>> UpdateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntity = ProductConversion.ToEntity(product);
            var response = await Iproduct.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("product")]
        public async Task<ActionResult<eCommerceLibrary.Response.Responses>> DeleteProduct(ProductDTO product)
        {          
            var getEntity = ProductConversion.ToEntity(product);
            var response = await Iproduct.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
