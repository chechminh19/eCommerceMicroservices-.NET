using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDTO dto) => new()
        {
            NameProduct = dto.Name,
            DescriptionProduct = dto.Des,
            Quantity = dto.Quantity,
            Price = dto.Price,
        };
        public static ProductDTOList FromEntityNew(Product product) =>
           new(product.Id!,product.NameProduct!, product.DescriptionProduct!, product.Quantity, product.Price);

        public static IEnumerable<ProductDTOList> FromEntities(IEnumerable<Product> products) =>
            products.Select(p => new ProductDTOList(p.Id!,p.NameProduct!, p.DescriptionProduct!, p.Quantity, p.Price));
       
    }
}
