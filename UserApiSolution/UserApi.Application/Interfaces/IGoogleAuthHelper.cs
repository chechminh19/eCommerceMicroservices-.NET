using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.Application.Interfaces
{
    public interface IGoogleAuthHelper
    {
        string[] GetScopes();
        string ScopeToString();
        ClientSecrets GetClientSecrets();
    }
    
}
