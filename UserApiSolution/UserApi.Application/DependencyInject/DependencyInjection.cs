using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application;
using UserApi.Application.Interfaces;

namespace UserApi.Application.DependencyInject
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // Register Service
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
