using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.Interfaces;

namespace UserApi.Application
{
    public class GoogleAuthHelperService(IConfiguration configuration) : IGoogleAuthHelper
    {
        public ClientSecrets GetClientSecrets()
        {
            string clientId = configuration["Google:ClientId"]!;
            string clientSecret = configuration["Google:ClientSecret"]!;
            return new() { ClientId = clientId, ClientSecret = clientSecret };
        }

        public string[] GetScopes()
        {
            var scopes = new[]
            {
                Oauth2Service.Scope.Openid,
                Oauth2Service.Scope.UserinfoEmail,
                Oauth2Service.Scope.UserinfoProfile,
            };
            return scopes;
        }

        public string ScopeToString() => string.Join(", ", GetScopes());
        
    }
}
