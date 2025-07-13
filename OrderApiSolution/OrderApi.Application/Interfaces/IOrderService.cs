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
        Task<ResponsesService<object>> CreateAsync(OrderCreateDTO dto);
        Task<ResponsesService<object>> UpdateAsync(OrderUpdateDTO dto, int id);
        Task<ResponsesService<object>> DeleteAsync(int id);
        Task<ResponsesService<OrderDTO?>> GetByIdAsync(int id);
        Task<ResponsesService<IEnumerable<OrderDTO>>> GetAllAsync();
    }
}
