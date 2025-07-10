using eCommerceLibrary.Response;
using OrderApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ResponsesService> CreateAsync(OrderCreateDTO dto);
        Task<ResponsesService> UpdateAsync(OrderUpdateDTO dto);
        Task<ResponsesService> DeleteAsync(int id);
        Task<OrderDTO?> GetByIdAsync(int id);
        Task<IEnumerable<OrderDTO>> GetAllAsync();
    }
}
