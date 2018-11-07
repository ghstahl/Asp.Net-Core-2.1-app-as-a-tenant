using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ApiWebApp.Middleware
{
    public class PathAuthorizationPolicyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<PathPolicyConfig> _settings;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly PathAuthorizationPolicyMiddlewareOptions _options;
        public PathAuthorizationPolicyMiddleware(RequestDelegate next, PathAuthorizationPolicyMiddlewareOptions options,
            IServiceProvider serviceProvider, IOptions<PathPolicyConfig> settings, ILogger<PathAuthorizationPolicyMiddleware> logger)
        {
            _next = next;
            _options = options;
            _serviceProvider = serviceProvider;
            _settings = settings;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, IAuthorizationService authorizationService)
        {
            foreach (var record in _settings.Value.OptIn)
            {
                var policy = record.Policy;
                foreach (var path in record.Paths)
                {
                    if (httpContext.Request.Path.StartsWithSegments(path))
                    {
                        // gotcha.
                        var authorized = await authorizationService.AuthorizeAsync(
                            httpContext.User, new NullResource(), policy);
                        if (!authorized.Succeeded)
                        {
                            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }
                }
            }
            await _next(httpContext);
        }
    }
}
