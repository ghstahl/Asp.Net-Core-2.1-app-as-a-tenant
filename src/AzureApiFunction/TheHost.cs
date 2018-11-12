using System;
using System.Collections.Generic;
using System.IO;
using ApiWebApp;
using Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tenant.Core;

namespace AzureApiFunction
{
    public static class TheHost
    {
        private static Dictionary<string, IServerRecord> _serversRecords;

        public static Dictionary<string, IServerRecord> GetServersRecords(string functionAppDirectory, IConfiguration configuration, ILogger logger)
        {
            if (_serversRecords == null)
            {
                var configSection = configuration.GetSection("tenantOptions");
                TenantOptions tenantOptions = new TenantOptions();
                configSection.Bind(tenantOptions);

                var serverRecords = new Dictionary<string, IServerRecord>();
                foreach (var tenant in tenantOptions.Tenants)
                {
                    serverRecords.Add(tenant.Name,
                        new ServerRecord<ApiWebApp.Startup>(functionAppDirectory, tenant.Name, logger)
                        {
                            BaseUrl = tenant.BaseUrl,
                            PathStringBaseUrl = new PathString(tenant.BaseUrl)
                        });
                }
                _serversRecords = serverRecords;
            }
            return _serversRecords;
        }
    }
}