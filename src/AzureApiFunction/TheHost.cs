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
        private static Dictionary<string, IServerRecord> _serversRecords;

        public static Dictionary<string, IServerRecord> GetServersRecords(string functionAppDirectory, ILogger logger)
        {
            if (_serversRecords == null)
            {
                var serverRecords = new Dictionary<string, IServerRecord>
                {
                    {
                        "simpleapiwebapp",
                        new ServerRecord<SimpleApiWebApp.Startup>(functionAppDirectory, "simpleapiwebapp", logger)
                    },
                    {
                        "apiwebapp",
                        new ServerRecord<ApiWebApp.Startup>(functionAppDirectory, "apiwebapp", logger)
                    }
                };
                _serversRecords = serverRecords;
            }
            return _serversRecords;
        }
  
    }
}