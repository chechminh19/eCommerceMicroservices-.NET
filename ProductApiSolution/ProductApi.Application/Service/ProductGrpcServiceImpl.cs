using Grpc.Core;
using ProductApi.Application.Interfaces;
using ProductGrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.Service
{
    public class ProductGrpcServiceImpl : ProductGrpcService.ProductGrpcServiceBase
    {
        private readonly IProductService _productService;
        public ProductGrpcServiceImpl(IProductService productService)
        {
            _productService = productService;
        }
        public override async Task<GetProductByIdResponse> GetProductById(GetProductByIdRequest request,
                                 ServerCallContext context)
        {
            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null || product.Data == null)
            {
                return new GetProductByIdResponse
                {
                    IsAvailable = false
                };
            }

            return new GetProductByIdResponse
            {
                ProductId = product.Data.Id,
                Name = product.Data.Name,
                Price = (double)product.Data.Price,
                Stock = product.Data.Quantity,
                IsAvailable = product.Data.Quantity > 0
            };
        }               
    }
}

