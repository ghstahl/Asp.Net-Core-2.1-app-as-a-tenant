using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace XUnitTenantHost
{
    public class TestDefaultHttpClientFactory : IDefaultHttpClientFactory
    {
        public static TestServer TestServer { get; set; }
        public HttpMessageHandler HttpMessageHandler => TestServer.CreateHandler();
        public HttpClient HttpClient => TestServer.CreateClient();
    }
}