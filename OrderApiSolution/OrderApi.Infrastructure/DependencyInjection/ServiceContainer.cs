using eCommerceLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure;
using OrderApi.Infrastructure.Repositories;


namespace OrderApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            //Add database connect
            //Add authen scheme
            SharedServiceContainer.AddSharedServices<OrderContext>(services, configuration, configuration["MySerilog:OrderService"]!);
            //Create DI
            services.AddScoped<IOrderRepo, OrderRepo>();

            return services;
        }
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register middleware such as:
            //Global Exception: handles external errors
            //listen to only Api gateway: block all outside calls
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
