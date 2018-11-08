using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace AzureApiFunction
{
    public static class TheHostConfiguration
    {
        private static IConfiguration _configuration;

       public static IConfiguration GetConfiguration(ExecutionContext context)
        {
            if (_configuration == null)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            }
            return _configuration;
        }
    }
}

