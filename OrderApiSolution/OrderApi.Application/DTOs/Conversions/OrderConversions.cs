using OrderApi.Application.Enums;
using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversions
    {
        public static Order ToEntity(OrderCreateDTO dto) => new()
        {
            UserId = dto.UserId,
            Status = dto.Status,
            PaymentDate = dto.Status == (byte)OrderEnums.Completed ? DateTime.Now : null,
            OrderDetails = dto.OrderDetails.Select(od => new OrderDetail
            {
                ProductId = od.ProductId,
                QuantityProduct = od.Quantity,
                Price = od.Price
            }).ToList()
        };

        public static Order ToEntityUpdate(OrderUpdateDTO dto, Order exist)
        {

            exist.UserId = dto.UserId;
            exist.Status = dto.Status;
            exist.CodePay = dto.CodePay;

            return exist;
        }

        public static OrderDTO ToDTO(Order entity) => new(
            entity.Id,
            entity.UserId,
            entity.Status,
            entity.CodePay,
            entity.PaymentDate
        );

        public static IEnumerable<OrderDTO> ToDTOs(IEnumerable<Order> orders)
            => orders.Select(o => ToDTO(o));

        public static OrderDetailDTO ToDetailDTO(OrderDetail entity) => new(
            entity.Id,
            entity.ProductId,
            entity.QuantityProduct,
            entity.Price,
            entity.Price * entity.QuantityProduct
        );

        public static OrderWithDetailsDTO ToDetailDTO(Order entity) => new(
            entity.Id,
            entity.UserId,
            entity.Status,
            entity.CodePay, 
            entity.PaymentDate,
            entity.OrderDetails?.Select(od => ToDetailDTO(od)).ToList() ?? new List<OrderDetailDTO>(),
            entity.OrderDetails?.Sum(od => od.Price * od.QuantityProduct) ?? 0
        );
    }
}
