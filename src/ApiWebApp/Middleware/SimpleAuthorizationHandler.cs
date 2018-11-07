using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ApiWebApp.Middleware
{
    public class SimpleAuthorizationHandler :
        AuthorizationHandler<IsAuthenticatedAuthorizationRequirement, HttpRequestResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            IsAuthenticatedAuthorizationRequirement requirement,
            HttpRequestResource resource)
        {
            if (context.User.Identity != null)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}