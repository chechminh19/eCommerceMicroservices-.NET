using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    // DTO khi TẠO đơn hàng mới
    public record OrderCreateDTO(
        [Required] int UserId,
        [Required] byte Status,
        [Required] List<OrderDetailCreateDTO> OrderDetails
    );
    public record OrderDetailCreateDTO(
    [Required] int ProductId,
    [Required] int Quantity,
    [Required] decimal Price
    );
    // DTO khi UPDATE đơn hàng
    public record OrderUpdateDTO(
        [Required] int UserId,
        [Required] byte Status,
        int? CodePay
    );

    // DTO khi TRẢ VỀ client
    public record OrderDTO(
        int Id,
        int UserId,
        byte Status,
        int? CodePay,
        DateTime? PaymentDate
    );
    public record OrderDetailDTO(
    int Id,
    int ProductId,
    int Quantity,
    decimal Price,
    decimal TotalPrice
    );
    public record OrderWithDetailsDTO(
        int Id,
        int UserId,
        byte Status,
        int? CodePay,
        DateTime? PaymentDate,
        List<OrderDetailDTO> OrderDetails,
        decimal TotalAmount
    );
}
