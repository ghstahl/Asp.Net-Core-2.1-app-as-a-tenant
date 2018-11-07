using Microsoft.AspNetCore.Builder;

namespace ApiWebApp.Middleware
{
    public static class PathAuthorizationPolicyMiddlewareExtensions
    {
        public static IApplicationBuilder UsePathAuthorizationPolicyMiddleware(this IApplicationBuilder builder, PathAuthorizationPolicyMiddlewareOptions options)
        {
            return builder.UseMiddleware<PathAuthorizationPolicyMiddleware>(options);
        }
    }
}