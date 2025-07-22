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
        Task<ResponsesService<object>> CreateAsync(ProductDTO dto);
        Task<ResponsesService<object>> UpdateAsync(ProductDTO dto, int id);
        Task<ResponsesService<int>> DeleteAsync(int id);
        Task<ResponsesService<ProductDTOList?>> GetByIdAsync(int id);
        Task<ResponsesService<PaginationModel<IEnumerable<ProductDTOList>>>> GetAllAsync(int page, int pageSize, string search, string sort);
    }
}
