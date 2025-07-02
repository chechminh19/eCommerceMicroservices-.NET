using eCommerceLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, 
            IConfiguration configuration, string fileName) where TContext: DbContext
        {
            // add generic database context
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                configuration.GetConnectionString("DatabaseConnection"), sqlOption => sqlOption.EnableRetryOnFailure()));
                
            // config serilog logging'
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text", 
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day).CreateLogger();
            //Add jwt schme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, configuration);
            return services;
        }
        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            //Use global exception
            app.UseMiddleware<GlobalException>();
            //Register middleware to block all outside calls
            //app.UseMiddleware<ListenToOnlyApiGateway>();
            return app;
        } 
    }

}
