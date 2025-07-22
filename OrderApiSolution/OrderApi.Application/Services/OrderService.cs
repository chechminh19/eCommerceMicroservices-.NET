using eCommerceLibrary.Response;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Enums;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
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

        public async Task<ResponsesService<object>> DeleteAsync(int id)
        {
            var order = await _orderRepo.FindByIdAsync(id);
            if (order == null)
                return ResponsesService<object>.Fail("Order not found", 404, id);

            bool deleteResult;
            try
            {
                deleteResult = await _orderRepo.DeleteAsync(order);
            }
            catch (Exception)
            {
                return ResponsesService<object>.Fail("Internal server error during delete", 500, id);
            }

            if (!deleteResult)
                return ResponsesService<object>.Fail("Failed to delete order due to conflict or constraints", 409, id);

            return ResponsesService<object>.Success("Order deleted successfully", id);
        }

        public async Task<ResponsesService<IEnumerable<OrderDTO>>> GetAllAsync()
        {
            try
            {
                var orders = await _orderRepo.GetAllAsync();
                var orderDto = OrderConversions.ToDTOs(orders);

                return ResponsesService<IEnumerable<OrderDTO>>.Success("Orders retrieved successfully", 200, orderDto);
            }
            catch (Exception)
            {
                return ResponsesService<IEnumerable<OrderDTO>>.Fail("Failed to retrieve orders", 500, Enumerable.Empty<OrderDTO>()
                );
            }
        }

        public async Task<ResponsesService<OrderDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepo.FindByIdAsync(id);

                if (order == null)
                    return ResponsesService<OrderDTO?>.Fail("Order not found", 404, null);

                var orderDto = OrderConversions.ToDTO(order);
                return ResponsesService<OrderDTO?>.Success("Order retrieved successfully", 200, orderDto);
            }
            catch (Exception)
            {
                return ResponsesService<OrderDTO?>.Fail("Failed to retrieve order", 500, null);
            }
        }

        public async Task HandleUserCreatedAsync(UserCreatedEvent evt)
        {
            if (await _orderRepo.FindCartByUserIdAsync(evt.UserId) != null)
            {
                Console.WriteLine($"User {evt.UserId} already have cart!");
                return;
            }

            var order = new Order
            {
                UserId = evt.UserId,
                Status = (byte)OrderEnums.Processing,
                OrderDetails = new List<OrderDetail>()
            };

            await _orderRepo.CreateAsync(order);
            Console.WriteLine($"Order created for UserId {evt.UserId}");
        }

        public async Task<ResponsesService<object>> UpdateAsync(OrderUpdateDTO dto, int id)
        {
            try
            {
                var existingOrder = await _orderRepo.FindByIdAsync(id);
                if (existingOrder == null)
                    return  ResponsesService<object>.Fail("Order not found", 404);

                existingOrder.UserId = dto.UserId;
                existingOrder.Status = dto.Status;
                existingOrder.CodePay = dto.CodePay;

                if (dto.Status == (byte)OrderEnums.Completed && existingOrder.PaymentDate == null)
                {
                    existingOrder.PaymentDate = DateTime.UtcNow;
                }
                else if (dto.Status != (byte)OrderEnums.Completed)
                {
                    existingOrder.PaymentDate = null;
                }

                await _orderRepo.UpdateAsync(OrderConversions.ToEntityUpdate(dto,existingOrder));
                return  ResponsesService<object>.Success("Order updated successfully", 200);
            }
            catch (Exception)
            {
                return  ResponsesService<object>.Fail("Error updating order", 500);
            }
        }

        
    }
}
