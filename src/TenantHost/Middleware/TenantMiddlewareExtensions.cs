using Microsoft.AspNetCore.Builder;

namespace TenantHost.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TenantMiddleware>();
        }
    }
}
