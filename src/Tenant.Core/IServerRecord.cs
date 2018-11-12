using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;

namespace Tenant.Core
{
    public interface IServerRecord
    {
        TestServer TestServer { get; }
        string ServerName { get; }
        string BaseUrl { get; }
        PathString PathStringBaseUrl { get; set; }
    }
}