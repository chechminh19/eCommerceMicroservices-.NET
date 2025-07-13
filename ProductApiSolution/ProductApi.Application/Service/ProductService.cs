using eCommerceLibrary.Response;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ResponsesService<object>> CreateAsync(ProductDTO dto)
        {
            try
            {
                var exist = await _productRepo.GetByAsync(p => p.NameProduct == dto.Name);
                if (exist != null)
                    return ResponsesService<object>.Fail($"{exist.NameProduct} already exists", 400);

                var entity = ProductConversion.ToEntity(dto);
                await _productRepo.CreateAsync(entity);

                return ResponsesService<object>.Success($"{entity.NameProduct} created successfully", 201);
            }catch (Exception)
            {
                return ResponsesService<object>.Fail("An unexpected error occurred.", 500);
            }
        }

        public async Task<ResponsesService<object>> UpdateAsync(ProductDTO dto, int id)
        {
            var existing = await _productRepo.FindByIdAsync(id);
            if (existing == null)
                return  ResponsesService<object>.Fail($"Product {id} not found", 404);

            var entity = ProductConversion.ToEntity(dto);
            await _productRepo.UpdateAsync(entity);

            return  ResponsesService<object>.Success($"{entity.NameProduct} updated", 204);
        }

        public async Task<ResponsesService<int>> DeleteAsync(int id)
        {
            var product = await _productRepo.FindByIdAsync(id);
            if (product == null)
                return  ResponsesService<int>.Fail($"Product {id} not found",404);
            bool deleteResult;
            try
            {
                deleteResult = await _productRepo.DeleteAsync(product);
            } catch (Exception)
            {
                return ResponsesService<int>.Fail("Internal server error during delete", 500, id);
            }
            return  ResponsesService<int>.Success($"Product {id} deleted", 204);
        }

        public async Task<ResponsesService<ProductDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productRepo.FindByIdAsync(id);

                if (product == null)
                    return ResponsesService<ProductDTO?>.Fail("Product not found", 404, null);

                var productDto = ProductConversion.FromEntityNew(product);
                return ResponsesService<ProductDTO?>.Success("Product not found", 200, productDto);
            }
            catch (Exception)
            {
                return ResponsesService<ProductDTO?>.Fail("Failed to retrieve product", 500, null);
            }
        }

        public async Task<ResponsesService<IEnumerable<ProductDTO>>> GetAllAsync()
        {
            try
            {
                var products = await _productRepo.GetAllAsync();
                var productsDto = ProductConversion.FromEntities(products);

                return ResponsesService<IEnumerable<ProductDTO>>.Success("Products retrieved successfully", 200, productsDto);
            }
            catch (Exception)
            {
                return ResponsesService<IEnumerable<ProductDTO>>.Fail("Failed to retrieve products", 500, Enumerable.Empty<ProductDTO>());
            }
        }       
    }
}
