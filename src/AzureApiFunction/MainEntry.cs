using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApiWebApp;
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

namespace AzureApiFunction
{

    public static class MainEntry
    {
        [FunctionName("MainEntry")]
        public static async Task<HttpResponseMessage> Run(
            ExecutionContext context,
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "{*all}")]
            HttpRequest req,
            ILogger log)
        {
            var path = req.Path.Value + req.QueryString.Value;
            log.LogInformation($"C# HTTP trigger:{req.Method} {path}.");

            HttpClient client = TheHost.GetServer(context, req, log).CreateClient();
            client.BaseAddress = new Uri($"{req.Scheme}://{req.Host}/");
            foreach (var header in req.Headers)
            {
                IEnumerable<string> values = header.Value;
                if (header.Key == "Host")
                {
                    continue;
                }
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, values);
            }

            HttpResponseMessage response = null;
            if (req.Method == "GET")
            {
                response = await client.GetAsync(path);
            }

            if (req.Method == "POST")
            {
                HttpContent content = null;
                if (req.ContentType == "application/x-www-form-urlencoded")
                {
                    var query = from item in req.Form
                        let c = new KeyValuePair<string, string>(item.Key, item.Value)
                        select c;
                    content = new FormUrlEncodedContent(query);
                }

                if (content == null)
                {
                    throw new UnsupportedContentTypeException($"{req.ContentType}");
                }
                response = await client.PostAsync(path, content);
            }

            return response;
        }
    }
}
