using eCommerceLibrary.Response;
using OrderApi.Application.DTOs;


namespace OrderApi.Application.Interfaces
{
    public interface IOrderService
    {
        Task HandleUserCreatedAsync(UserCreatedEvent evt);
        //Task<ResponsesService<object>> CreateAsync(OrderCreateDTO dto);
        Task<ResponsesService<object>> UpdateAsync(OrderUpdateDTO dto, int id);
        Task<ResponsesService<object>> DeleteAsync(int id);
        Task<ResponsesService<OrderDTO?>> GetByIdAsync(int id);
        Task<ResponsesService<IEnumerable<OrderDTO>>> GetAllAsync();
    }
}
