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
            Id = dto.Id,
            NameProduct = dto.Name,
            DescriptionProduct = dto.Des,
            Quantity = dto.Quantity,
            Price = dto.Price,
        };
        public static ProductDTO FromEntityNew(Product product) =>
           new(product.Id, product.NameProduct!, product.DescriptionProduct!, product.Quantity, product.Price);

        public static IEnumerable<ProductDTO> FromEntities(IEnumerable<Product> products) =>
            products.Select(p => new ProductDTO(p.Id, p.NameProduct!, p.DescriptionProduct!, p.Quantity, p.Price));
        //public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        //{
        //    // if single
        //    if(product is not null || products is null)
        //    {
        //        var productSingle = new ProductDTO(product!.Id, 
        //            product.NameProduct!, 
        //            product.DescriptionProduct!,
        //            product.Quantity,
        //            product.Price);
        //        return (productSingle, null);
        //    }
        //    // if list
        //    if (products is not null || product is null)
        //    {
        //        var _products = products.Select(p => new ProductDTO(p.Id, p.NameProduct, p.DescriptionProduct, p.Quantity, p.Price)).ToList();
        //        return (null, _products);                              
        //    }
        //    return(null,null);
        //}
    }
}
