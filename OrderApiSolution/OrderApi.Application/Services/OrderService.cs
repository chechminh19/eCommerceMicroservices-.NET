using eCommerceLibrary.Response;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Enums;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using ProductGrpc;
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
        private readonly ProductGrpcService.ProductGrpcServiceClient _productClient;
        public OrderService(IOrderRepo orderRepo, ProductGrpcService.ProductGrpcServiceClient productClient)
        {
            _orderRepo = orderRepo;
            _productClient = productClient;
        }

        public async Task<ResponsesService<object>> AddProductToOrder(int orderId, int productId, int quantity)
        {
            var order = await _orderRepo.FindByIdAsync(orderId);
            if (order == null)
                return ResponsesService<object>.Fail("Order not found.", 404);

            var productResponse = await _productClient.GetProductByIdAsync(new GetProductByIdRequest { ProductId = productId });
            if(productResponse.ProductId == 0 || !productResponse.IsAvailable)
            {
                if (productResponse.ProductId == 0)
                    return ResponsesService<object>.Fail($"Product with ID {productId} not found.", 404);
                else
                    return ResponsesService<object>.Fail("Product is not available.", 400);
            }

            if (productResponse.Stock < quantity)
                return ResponsesService<object>.Fail("Product stock insufficient.", 400);


            order.OrderDetails ??= new List<OrderDetail>();
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = productResponse.ProductId,
                QuantityProduct = quantity,
                Price = (decimal)productResponse.Price,
                OrderId = order.Id
            });

            await _orderRepo.UpdateAsync(order);
            return ResponsesService<object>.Success("Product added to order successfully.", 200);
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

        public async Task<ResponsesService<OrderWithDetailsDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepo.FindByIdAsync(id);

                if (order == null)
                    return ResponsesService<OrderWithDetailsDTO?>.Fail("Order not found", 404, null);

                var orderDto = OrderConversions.ToDetailDTO(order);
                return ResponsesService<OrderWithDetailsDTO?>.Success("Order retrieved successfully", 200, orderDto);
            }
            catch (Exception)
            {
                return ResponsesService<OrderWithDetailsDTO?>.Fail("Failed to retrieve order", 500, null);
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
