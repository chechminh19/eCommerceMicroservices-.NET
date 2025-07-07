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
        int? CodePay
    );

    // DTO khi UPDATE đơn hàng
    public record OrderUpdateDTO(
        [Required] int Id,
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
}
