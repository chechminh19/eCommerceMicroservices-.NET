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

        public async Task<ResponsesService> CreateAsync(ProductDTO dto)
        {
            var exist = await _productRepo.GetByAsync(p => p.NameProduct == dto.Name);
            if (exist != null)
                return new ResponsesService(false, $"{exist.NameProduct} already exists");

            var entity = ProductConversion.ToEntity(dto);
            await _productRepo.CreateAsync(entity);

            return new ResponsesService(true, $"{entity.NameProduct} created successfully");
        }

        public async Task<ResponsesService> UpdateAsync(ProductDTO dto)
        {
            var existing = await _productRepo.FindByIdAsync(dto.Id);
            if (existing == null)
                return new ResponsesService(false, $"Product {dto.Id} not found");

            var entity = ProductConversion.ToEntity(dto);
            await _productRepo.UpdateAsync(entity);

            return new ResponsesService(true, $"{entity.NameProduct} updated");
        }

        public async Task<ResponsesService> DeleteAsync(int id)
        {
            var product = await _productRepo.FindByIdAsync(id);
            if (product == null)
                return new ResponsesService(false, $"Product {id} not found");

            await _productRepo.DeleteAsync(product);
            return new ResponsesService(true, $"Product {id} deleted");
        }

        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            var entity = await _productRepo.FindByIdAsync(id);
            return entity is null ? null : ProductConversion.FromEntityNew(entity);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var list = await _productRepo.GetAllAsync();
            return ProductConversion.FromEntities(list);
        }
    }
}
