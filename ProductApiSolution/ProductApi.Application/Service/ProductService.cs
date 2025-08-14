using eCommerceLibrary.Response;
using eCommerceLibrary.Utils;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductGrpc;

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

        public async Task<ResponsesService<ProductDTOList?>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productRepo.FindByIdAsync(id);

                if (product == null)
                    return ResponsesService<ProductDTOList?>.Fail("Product not found", 404, null);

                var productDto = ProductConversion.FromEntityNew(product);
                return ResponsesService<ProductDTOList?>.Success("Product here", 200, productDto);
            }
            catch (Exception)
            {
                return ResponsesService<ProductDTOList?>.Fail("Failed to retrieve product", 500, null);
            }
        }

        public async Task<ResponsesService<PaginationModel<IEnumerable<ProductDTOList>>>> GetAllAsync(int page, int pageSize, string search, string sort)
        {
            try
            {
                var products = await _productRepo.GetAllAsync();
                if(!string.IsNullOrEmpty(search))
                {
                    products = products.Where(p => p.NameProduct.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                products = sort.ToLower() switch
                {  // Sort by price in ascending order when "price" is specified
                    "price" => products.OrderBy(p => p.Price),
                    // Sort by name, material, color as required
                    "name" => products.OrderBy(p => p.NameProduct),                    
                    // Default to sorting by Id in descending order (newest first) when no sort or other sort types
                    _ => products.OrderByDescending(p => p.Id)
                };
                var productsDto = ProductConversion.FromEntities(products);
                var paginationDtos = await Pagination.GetPagination(productsDto, page, pageSize);
                return ResponsesService<PaginationModel<IEnumerable<ProductDTOList>>>.Success("Products retrieved successfully", 200, paginationDtos);
            }
            catch (Exception)
            {
                return ResponsesService<PaginationModel<IEnumerable<ProductDTOList>>>.Fail("Failed to retrieve products", 500, null);
            }
        }
        
    }
}
