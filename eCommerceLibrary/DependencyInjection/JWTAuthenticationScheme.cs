﻿using eCommerceLibrary.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceLibrary.DependencyInjection
{
    public static class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJWTAuthenticationScheme(this IServiceCollection services, IConfiguration configuration) 
        {
            //add jwt services
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(configuration.GetSection("Authentication:Key").Value!);
                    string issue = configuration.GetSection("Authentication:Issuer").Value!;
                    string audience = configuration.GetSection("Authentication:Audience").Value!;

                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issue,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
            return services;
        }     
        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
            app.UseMiddleware<ListenToOnlyApiGateway>();
            return app;
        } 
    }
}
