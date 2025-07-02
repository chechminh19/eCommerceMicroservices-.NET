using eCommerceLibrary.Response;
using ProductApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ResponsesService> CreateAsync(ProductDTO dto);
        Task<ResponsesService> UpdateAsync(ProductDTO dto);
        Task<ResponsesService> DeleteAsync(int id);
        Task<ProductDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetAllAsync();
    }
}
