using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace AzureApiFunction
{
    public class SomeObject
    {
    }

    public static class TheHostConfiguration
    {
        private static IConfiguration _configuration;

        public static IConfiguration GetConfiguration(string functionAppDirectory)
        {
            if (_configuration == null)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var builder = new ConfigurationBuilder()
                    .SetBasePath(functionAppDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true);
                if (environment == "Development")
                {
                    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                    builder.AddUserSecrets<SomeObject>();
                }
                builder.AddEnvironmentVariables();


                _configuration = builder.Build();
            }

            return _configuration;
        }
    }
}


