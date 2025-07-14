﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain.Entities;

namespace UserApi.Application.Utils
{
    public static class GenerateJsonWebTokenString
    {
        public static string GenerateJsonWebToken(this User user,string issuer,string audience,string secretKey,DateTime now)
        {
            if (Encoding.UTF8.GetBytes(secretKey).Length < 32)
            {
                // Adjust key length to 32 bytes (256 bits) using padding if necessary
                secretKey = secretKey.PadRight(32, '0');
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Email" ,user.Email),
                new Claim(ClaimTypes.Role ,user.Role.ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: now.AddHours(3),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
