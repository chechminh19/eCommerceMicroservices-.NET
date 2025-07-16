using Azure.Core;
using eCommerceLibrary.Response;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.DTOs;
using UserApi.Application.Interfaces;
using UserApi.Application.Utils;
using UserApi.Domain.Entities;
using UserApi.Infrastructure;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
namespace UserApi.Application
{
    public class GoogleAuthorService : IGoogleAuthor
    {
        private readonly UserContext _context;
        private readonly IConfiguration _configuration;
        private string _redirectUrl;
        private readonly IGoogleAuthHelper _authHelper;
        public GoogleAuthorService(UserContext context, IConfiguration configuration, IGoogleAuthHelper googleAuthHelper) 
        {
            _context = context; 
            _configuration = configuration;
            _redirectUrl = configuration["Google:RedirectUri"];
            _authHelper = googleAuthHelper;
        }

        public async Task<ResponsesService<LoginResponseGoogleDTO>> ExchangeCodeForToken(string code)
        {
            try
            {
                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = _authHelper.GetClientSecrets(),
                    Scopes = _authHelper.GetScopes(),
                });

                var tokenResponse = await flow.ExchangeCodeForTokenAsync(
                    userId: null,
                    code: code,
                    redirectUri: _redirectUrl,
                    CancellationToken.None);

                var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken);

                var user = await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

                if (user == null)
                {
                    user = await _context.Set<User>()
                        .FirstOrDefaultAsync(u => u.Email == payload.Email);

                    if (user != null)
                    {
                        return ResponsesService<LoginResponseGoogleDTO>.Fail(
                            "Email has been registered before. Please login with Email/Password.", 409);
                    }

                    user = new User
                    {
                        GoogleId = payload.Subject,
                        Email = payload.Email,
                        FullName = payload.Name,
                        CreatedAt = DateTime.UtcNow,
                        IsEmailVerified = false
                    };
                    await _context.AddAsync(user);
                }

                user.LastLogin = DateTime.UtcNow;             
                var jwt = user.GenerateJsonWebToken(_configuration["Authentication:Issuer"]!,_configuration["Authentication:Audience"]!,_configuration["Authentication:Key"]!,DateTime.UtcNow);

                var response = new LoginResponseGoogleDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    JwtToken = jwt,
                    //GoogleAccessToken = tokenResponse.AccessToken,
                    //GoogleIdToken = tokenResponse.IdToken,                  
                };
                await _context.SaveChangesAsync();
                return ResponsesService<LoginResponseGoogleDTO>.Success("Google login successful.", 200, response);
            }
            catch (Exception)
            {
                return ResponsesService<LoginResponseGoogleDTO>.Fail("Google login failed.", 500);
            }
        }

        public string GetAuthorizationUrl()
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = _authHelper.GetClientSecrets(),
                Scopes = _authHelper.GetScopes(),
            });

            var req = flow.CreateAuthorizationCodeRequest(_redirectUrl);
            req.RedirectUri = _redirectUrl;
            return req.Build().ToString();
        }

        public async Task<User> ValidCodeForToken(string accessToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(accessToken);

            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

            if (user == null)
            {
                user = new User
                {
                    GoogleId = payload.Subject,
                    Email = payload.Email,
                    FullName = payload.Name,
                    CreatedAt = DateTime.UtcNow,
                    IsEmailVerified = false
                };
                await _context.AddAsync(user);
            }

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
