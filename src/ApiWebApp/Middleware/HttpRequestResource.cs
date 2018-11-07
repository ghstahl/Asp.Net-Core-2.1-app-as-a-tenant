using Microsoft.AspNetCore.Http;

namespace ApiWebApp.Middleware
{
    public class HttpRequestResource
    {
        public HttpRequest Request { get; set; }
    }
}