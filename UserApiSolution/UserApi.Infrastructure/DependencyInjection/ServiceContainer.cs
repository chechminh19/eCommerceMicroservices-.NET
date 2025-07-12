using eCommerceLibrary.DependencyInjection;
using eCommerceLibrary.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Application.Interfaces;
using UserApi.Infrastructure;
using UserApi.Infrastructure.Repo;


namespace UserApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            //Add database connect
            //Add authen scheme
            SharedServiceContainer.AddSharedServices<UserContext>(services, configuration, configuration["MySerilog:UserService"]!);
            //Create DI
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<ITransactionRepo, UserRepo>();

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
