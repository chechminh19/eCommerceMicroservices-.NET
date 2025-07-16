using eCommerceLibrary.Response;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApi.Application.DTOs;
using UserApi.Domain.Entities;

namespace UserApi.Application.Interfaces
{
    public interface IGoogleAuthor
    {
        string GetAuthorizationUrl();
        Task<ResponsesService<LoginResponseGoogleDTO>> ExchangeCodeForToken(string code);
        Task<User> ValidCodeForToken(string accessToken);
    }
}
