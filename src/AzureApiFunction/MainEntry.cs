using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApiWebApp;
using Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Tenant.Core.Shims;

namespace AzureApiFunction
{

    public static class MainEntry
    {
        [FunctionName("MainEntry")]
        public static async Task<HttpResponseMessage> Run(
            ExecutionContext context,
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", "delete", Route = "{*all}")]
            HttpRequest request,
            ILogger log)
        {

            var configuration = TheHostConfiguration.GetConfiguration(context.FunctionAppDirectory);
            var serverRecords = TheHost.GetServersRecords(context.FunctionAppDirectory, configuration,log);

            var path = new PathString(request.Path.Value.ToLower());
            log.LogInformation($"C# HTTP trigger:{request.Method} {path}.");

            var query = from item in serverRecords
                        where path.StartsWithSegments(item.Value.PathStringBaseUrl)
                select item.Value;
            var serverRecord = query.FirstOrDefault();
            HttpResponseMessage response = null;
            if (serverRecord == null)
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }
            

            var httpRequestMessageFeature = new TenantHttpRequestMessageFeature(request);
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
                uriBuilder.Port = (int)request.Host.Port;
            }

            httpRequestMessage.RequestUri = uriBuilder.Uri;
            httpRequestMessage.Headers.Remove("Host");
            var responseMessage = await client.SendAsync(httpRequestMessage);
            return responseMessage;
        }
    }
}
