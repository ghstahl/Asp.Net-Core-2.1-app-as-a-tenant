using System.Net.Http;

namespace XUnitTenantHost
{
    public interface IDefaultHttpClientFactory
    {
        HttpMessageHandler HttpMessageHandler { get; }
        HttpClient HttpClient { get; }
    }
}