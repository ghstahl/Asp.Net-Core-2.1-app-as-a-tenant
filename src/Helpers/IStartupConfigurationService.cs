﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Helpers
{
    public interface IStartupConfigurationService
    {
        void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);

        void ConfigureEnvironment(IHostingEnvironment env);

        void ConfigureService(IServiceCollection services, IConfigurationRoot configuration);
    }
}
