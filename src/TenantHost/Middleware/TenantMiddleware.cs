using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Tenant.Core;

namespace TenantHost.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private IOptions<TenantOptions> _optionsAccessor;
        private List<IServerRecord> _serversRecords;
        private ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;

        public TenantMiddleware(
            IHostingEnvironment hostingEnvironment,
            ILogger<TenantMiddleware> logger,
            IOptions<TenantOptions> optionsAccessor, 
            RequestDelegate next)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _optionsAccessor = optionsAccessor;
            _next = next;
            _serversRecords = GetServersRecords(_hostingEnvironment.ContentRootPath, _logger);
        }

        private List<IServerRecord> GetServersRecords(string functionAppDirectory, ILogger logger)
        {

            var serverRecords = new List<IServerRecord>();
            foreach (var tenant in _optionsAccessor.Value.Tenants)
            {
                serverRecords.Add( 
                    new ServerRecord<ApiWebApp.Startup>(tenant.Name,functionAppDirectory, tenant.SettingsPath, logger)
                    {
                        BaseUrl = tenant.BaseUrl,
                        PathStringBaseUrl = new PathString(tenant.BaseUrl)
                    });
            }

            return serverRecords;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                var request = httpContext.Request;
                var path = request.Path;
                var query = from item in _serversRecords
                    where path.StartsWithSegments(item.PathStringBaseUrl)
                    select item;
                var serverRecord = query.FirstOrDefault();
                if (serverRecord != null)
                {
                    var httpRequestMessageFeature = new HttpRequestMessageFeature(httpContext);
                    var httpRequestMessage = httpRequestMessageFeature.HttpRequestMessage;

                    HttpClient client = serverRecord.TestServer.CreateClient();
                    client.BaseAddress = new Uri($"{request.Scheme}://{request.Host}");

                    // trim off the front router hints
                    path = path.Value.Substring(serverRecord.PathStringBaseUrl.Value.Length);
                    var uriBuilder = new UriBuilder(request.Scheme, request.Host.Host)
                    {
                        Path = path,
                        Query = request.QueryString.Value
                    };
                    if (request.Host.Port != null)
                    {
                        uriBuilder.Port = (int) request.Host.Port;
                    }

                    httpRequestMessage.RequestUri = uriBuilder.Uri;
                    httpRequestMessage.Headers.Remove("Host");
                    var responseMessage = await client.SendAsync(httpRequestMessage);

                    httpContext.Response.StatusCode = (int)responseMessage.StatusCode;
                    foreach (var header in responseMessage.Headers)
                    {
                        httpContext.Response.Headers[header.Key] = header.Value.ToArray();
                    }

                    foreach (var header in responseMessage.Content.Headers)
                    {
                        httpContext.Response.Headers[header.Key] = header.Value.ToArray();
                    }

                    // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
                    httpContext.Response.Headers.Remove("transfer-encoding");
                    await responseMessage.Content.CopyToAsync(httpContext.Response.Body);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception: {exception}", ex.Message);
                throw;
            }
            await _next(httpContext);
        }
    }
}