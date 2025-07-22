using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs
{
    public record ProductDTO
    (
        [Required] [MaxLength(20)] string Name,
        [Required] [MaxLength(20)] string Des,
        [Required, Range(1, int.MaxValue)] int Quantity,
        [Required, DataType(DataType.Currency)] decimal Price
        );   
    public record ProductDTOList
    (
        int id,
        string Name,
        string Des,
        int Quantity,
        decimal Price
        );

}
