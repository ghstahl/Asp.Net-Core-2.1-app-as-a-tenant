using System;
using System.Collections.Generic;
using System.IO;
using ApiWebApp;
using Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureApiFunction
{
    public static class TheHost
    {
        private static Dictionary<string, ServerRecord> _serversRecords;

        private static ServerRecord BuildServerRecord(Microsoft.AspNetCore.Http.HttpRequest req, string key,ExecutionContext context,ILogger log )
        {
            var serverRecord = new ServerRecord()
            {
                Server = key
            };
            var webHostBuilder = new WebHostBuilder()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddProvider(new MyLoggerProvider(log));
                })
                .UseContentRoot($"{context.FunctionAppDirectory}/{serverRecord.Server}");
            switch (key)
            {
                case "simpleapiwebapp":
                    webHostBuilder.UseStartup<SimpleApiWebApp.Startup>();
                    break;
                case "apiwebapp":
                    webHostBuilder.UseStartup<ApiWebApp.Startup>();
                    break;
                default:
                    return null;
            }
            webHostBuilder.ConfigureServices(s =>
                s.AddSingleton<IStartupConfigurationService, NullStartupConfigurationService>());
            serverRecord.WebHostBuilder = webHostBuilder;
            serverRecord.BaseAddress =  req.Scheme + "://" + req.Host.ToUriComponent();
            return serverRecord;
        }

        public static Dictionary<string, ServerRecord> GetServerRecords(
            Microsoft.AspNetCore.Http.HttpRequest req, 
            ExecutionContext context, ILogger log)
        {
            if (_serversRecords == null)
            {
                var serverRecords = new Dictionary<string, ServerRecord>
                {
                    {"simpleapiwebapp", BuildServerRecord(req,"simpleapiwebapp", context, log)},
                    {"apiwebapp", BuildServerRecord(req,"apiwebapp", context, log)}
                };
                _serversRecords = serverRecords;
            }
            return _serversRecords;
        }
    }
}