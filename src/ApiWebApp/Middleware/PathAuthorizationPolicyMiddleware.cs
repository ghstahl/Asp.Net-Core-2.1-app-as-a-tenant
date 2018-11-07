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
            var resource = new HttpRequestResource() {Request = httpContext.Request};
            if (_settings.Value.OptIn != null)
            {
                foreach (var record in _settings.Value.OptIn)
                {
                    var policy = record.Policy;
                    bool foundPerfectMatch = false;
                    var query = from item in record.Paths
                        where item == httpContext.Request.Path
                        select item;
                    if (!query.Any())
                    {
                        foundPerfectMatch = true;
                        // gotcha.
                        var authorized = await authorizationService.AuthorizeAsync(
                            httpContext.User, resource, policy);
                        if (!authorized.Succeeded)
                        {
                            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }

                    if (!foundPerfectMatch)
                    {
                        // match starting segments
                        query = from item in record.Paths
                            where item != "/" && httpContext.Request.Path.StartsWithSegments(item)
                            select item;
                        if (!query.Any())
                        {
                            // gotcha.
                            var authorized = await authorizationService.AuthorizeAsync(
                                httpContext.User, resource, policy);
                            if (!authorized.Succeeded)
                            {
                                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return;
                            }
                        }
                    }
                    
                }
            }
            if (_settings.Value.OptOut != null)
            {
                foreach (var record in _settings.Value.OptOut)
                {
                    // look for a perfect match
                    var policy = record.Policy;
                    bool foundPerfectMatch = false;
                    var query = from item in record.Paths
                        where item == httpContext.Request.Path
                        select item;
                    if (!query.Any())
                    {
                        foundPerfectMatch = true;
                        var authorized = await authorizationService.AuthorizeAsync(
                            httpContext.User, resource, policy);
                        if (!authorized.Succeeded)
                        {
                            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }

                    if (!foundPerfectMatch)
                    {
                        // match starting segments
                        query = from item in record.Paths
                            where item != "/" && !httpContext.Request.Path.StartsWithSegments(item)
                            select item;
                        if (!query.Any())
                        {
                            var authorized = await authorizationService.AuthorizeAsync(
                                httpContext.User, resource, policy);
                            if (!authorized.Succeeded)
                            {
                                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return;
                            }
                        }
                    }
                }
            }
            await _next(httpContext);
        }
    }
}
