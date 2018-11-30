using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiWebApp.Controllers;
using ApiWebApp.Middleware;
using ApiWebApp.Services;
using GraphQL;
using GraphQL.StartWars.Standard.Extensions;
using Helpers;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ApiWebApp
{

    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStartupConfigurationService _externalStartupConfiguration;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PathPolicyConfig>(Configuration.GetSection(PathPolicyConfig.WellKnown_SectionName));
            services.AddDictionaryCache();

            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

           

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddStarWarsTypes();

            // Pass configuration (IConfigurationRoot) to the configuration service if needed
            _externalStartupConfiguration.ConfigureService(services, null);

            var authority = Configuration["Authority"];

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "nitro";
                    options.ApiSecret = "secret";

                }); 
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAuthenticatedPolicy", policy =>
                    policy.Requirements.Add(new IsAuthenticatedAuthorizationRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, SimpleAuthorizationHandler>();
            services.RemoveAll<IConfiguration>();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRewriter(new RewriteOptions().Add(new RewriteLowerCaseRule()));
            _externalStartupConfiguration.Configure(app, env, loggerFactory);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();// redirect to https is more important than authentication, so it goes first
            app.UseAuthentication();  // make sure this is placed close to first in the pipeline.

            // app.UsePathAuthorizationPolicyMiddleware must come AFTER app.UseAuthentication();
            // app.UseAuthentication(); does the JWT stuff, and the things after rely on it.
            app.UsePathAuthorizationPolicyMiddleware(new PathAuthorizationPolicyMiddlewareOptions());
            app.UseMvc();

        }
        private void StartupConfiguration(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(_hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddConfiguration(configuration)/*put last, we want it to win*/;

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
