using System;
using ApiWebApp;
using Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureApiFunction
{
    public static class TheHost
    {
        private static TestServer Server;

        public static TestServer GetServer(ExecutionContext context, Microsoft.AspNetCore.Http.HttpRequest req, ILogger log)
        {
            try
            {
                if (Server == null)
                {

                    log.LogInformation("Creating TestServer");
                    var webHostBuilder = new WebHostBuilder()
                        .ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.ClearProviders();
                            logging.AddProvider(new MyLoggerProvider(log));
                        })
                        .UseContentRoot(context.FunctionAppDirectory)
                        .UseStartup<Startup>();
                    webHostBuilder.ConfigureServices(s => s.AddSingleton<IStartupConfigurationService, NullStartupConfigurationService>());

                    Server = new TestServer(webHostBuilder);
                    var baseAddress = req.Scheme + "://" + req.Host.ToUriComponent();
                    Server.BaseAddress = new Uri(baseAddress);
                }
                return Server;
            }
            catch (Exception e)
            {
                log.LogError(e, $"Creating Server Exception:{e.Message}");
                throw;
            }
        }
    }
}