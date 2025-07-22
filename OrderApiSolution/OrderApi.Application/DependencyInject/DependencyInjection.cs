using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Application.Services.Kafka;


namespace ProductApi.Application.DependencyInject
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // Register Service
            services.AddScoped<IOrderService, OrderService>();
            services.AddHostedService<KafkaConsumerUserCreatedService>();
            return services;
        }
    }
}
