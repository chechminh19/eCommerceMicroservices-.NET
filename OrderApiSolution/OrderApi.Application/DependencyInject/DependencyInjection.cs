using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Application.Services.Kafka;
using ProductGrpc;

namespace OrderApi.Application.DependencyInject
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            // Register Service
            services.AddScoped<IOrderService, OrderService>();
            var enableKafkaConsumer = config.GetValue<bool>("Features:EnableKafkaConsumer");
            if (enableKafkaConsumer)
            {
                services.AddHostedService<KafkaConsumerUserCreatedService>();
            }
            // gRPC client
            services.AddGrpcClient<ProductGrpcService.ProductGrpcServiceClient>(o =>
            {
                o.Address = new Uri(config["GrpcSettings:ProductServiceUrl"]);               
            });

            return services;
        }
    }
}
