using eCommerceLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            //Add database connect
            //Add authen scheme
            SharedServiceContainer.AddSharedServices<ProductContext>(services, configuration, configuration["MySerilog:FileName"]!);
            //Create DI
            services.AddScoped<IProduct, ProductRepo>();

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
