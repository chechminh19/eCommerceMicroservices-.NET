using eCommerceLibrary.Response;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Enums;
using OrderApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        public OrderService(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<ResponsesService> CreateAsync(OrderCreateDTO dto)
        {
            try
            {
                // Kiểm tra nếu order đã tồn tại (ví dụ: kiểm tra theo CodePay nếu cần)
                if (dto.CodePay.HasValue)
                {
                    var exist = await _orderRepo.GetByAsync(o => o.CodePay == dto.CodePay);
                    if (exist != null)
                        return new ResponsesService(false, $"Order with CodePay {dto.CodePay} already exists");
                }

                var entity = OrderConversions.ToEntity(dto);
                var createdOrder = await _orderRepo.CreateAsync(entity);

                return new ResponsesService(true, $"Order {createdOrder.Id} created successfully");
            }
            catch (Exception ex)
            {
                return new ResponsesService(false, $"Error creating order: {ex.Message}");
            }
        }

        public async Task<ResponsesService> DeleteAsync(int id)
        {
            try
            {
                var order = await _orderRepo.FindByIdAsync(id);
                if (order == null)
                    return new ResponsesService(false, "Order not found");

                var result = await _orderRepo.DeleteAsync(order);
                return result
                    ? new ResponsesService(true, $"Order {id} deleted successfully")
                    : new ResponsesService(false, $"Failed to delete order {id}");
            }
            catch (Exception ex)
            {
                return new ResponsesService(false, $"Error deleting order: {ex.Message}");
            }
        }

        public async Task<ResponsesServiceDTO<IEnumerable<OrderDTO>>> GetAllAsync()
        {
            try
            {
                var orders = await _orderRepo.GetAllAsync();
                return new ResponsesServiceDTO<IEnumerable<OrderDTO>>(
                    true,
                    "Orders retrieved successfully",
                    OrderConversions.ToDTOs(orders));
            }
            catch (Exception ex)
            {
                return new ResponsesServiceDTO<IEnumerable<OrderDTO>>(
                    false,
                    $"Error retrieving orders: {ex.Message}",
                    null);
            }
        }

        public async Task<ResponsesServiceDTO<OrderDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepo.FindByIdAsync(id);
                if (order == null)
                    return new ResponsesServiceDTO<OrderDTO>(false, "Order not found", null);

                return new ResponsesServiceDTO<OrderDTO>(true, "Order found", OrderConversions.ToDTO(order));
            }
            catch (Exception ex)
            {
                return new ResponsesServiceDTO<OrderDTO>(false, $"Error retrieving order: {ex.Message}", null);
            }
        }

        public async Task<ResponsesService> UpdateAsync(OrderUpdateDTO dto)
        {
            try
            {
                var existingOrder = await _orderRepo.FindByIdAsync(dto.Id);
                if (existingOrder == null)
                    return new ResponsesService(false, "Order not found");

                // Cập nhật thông tin
                existingOrder.UserId = dto.UserId;
                existingOrder.Status = dto.Status;
                existingOrder.CodePay = dto.CodePay;

                // Xử lý PaymentDate tự động
                if (dto.Status == (byte)OrderEnums.Completed && existingOrder.PaymentDate == null)
                {
                    existingOrder.PaymentDate = DateTime.UtcNow;
                }
                else if (dto.Status != (byte)OrderEnums.Completed)
                {
                    existingOrder.PaymentDate = null;
                }

                await _orderRepo.UpdateAsync(existingOrder);
                return new ResponsesService(true, $"Order {dto.Id} updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponsesService(false, $"Error updating order: {ex.Message}");
            }
        }

        
    }
}
