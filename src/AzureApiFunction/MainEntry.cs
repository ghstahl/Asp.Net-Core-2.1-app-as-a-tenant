using System.Net.Http;
using System.Threading.Tasks;
using ApiWebApp;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureApiFunction
{
    class SomeObject { }
    public static class MainEntry
    {
        [FunctionName("MainEntry")]
        public static async Task<HttpResponseMessage> Run(
            ExecutionContext context,
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", "delete", Route = "{*all}")]
            HttpRequest request,
            ILogger log)
        {
            return await Tenant.Core.AzureFunctions.Entry<SomeObject, Startup>.Run(context, request, log);
        }
    }
}
