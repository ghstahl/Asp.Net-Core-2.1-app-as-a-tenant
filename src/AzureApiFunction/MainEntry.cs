using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ApiWebApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            HttpClient client = TheHost.GetServer().CreateClient();
       
         
            var dd = await client.GetAsync("/api/values");

            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return dd;
        }
    }
}
