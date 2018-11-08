using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SimpleApiWebApp
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment,
            IStartupConfigurationService externalStartupConfiguration)
        {
            _hostingEnvironment = hostingEnvironment;
            _externalStartupConfiguration = externalStartupConfiguration;
            _externalStartupConfiguration.ConfigureEnvironment(hostingEnvironment);
            StartupConfiguration(configuration);
        }

        public IConfiguration Configuration { get; set; }
        private readonly IHostingEnvironment _hostingEnvironment;

        private IStartupConfigurationService _externalStartupConfiguration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Pass configuration (IConfigurationRoot) to the configuration service if needed
            _externalStartupConfiguration.ConfigureService(services, null);

            services.RemoveAll<IConfiguration>();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
        private void StartupConfiguration(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(_hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_hostingEnvironment.EnvironmentName}.json", optional: true);

            if (_hostingEnvironment.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
    }
}
