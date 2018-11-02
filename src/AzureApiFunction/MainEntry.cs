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
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace AzureApiFunction
{
    public static class TheHost
    {
        private static TestServer Server;

        public static TestServer GetServer()
        {
            if (Server == null)
            {
                Server = new TestServer(new WebHostBuilder()
                      .UseStartup<Startup>());
            }

            return Server;
        }

    }
    public static class MainEntry
    {
        [FunctionName("MainEntry")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "MainEntry/{*all}")]
            HttpRequest req,
            ILogger log)
        {
            var path = req.Path.Value.Substring(10) + req.QueryString.Value;
            HttpClient client = TheHost.GetServer().CreateClient();
            foreach (var header in req.Headers)
            {
                IEnumerable<string> values = header.Value;
                client.DefaultRequestHeaders.Add(header.Key, values);
            }

            HttpResponseMessage response = null;
            if (req.Method == "GET")
            {
                response = await client.GetAsync(path);
            }

            if (req.Method == "POST")
            {
                response = await client.PostAsync(path,new StreamContent(req.Body));
            }
           
            return response;
        }
    }
}
