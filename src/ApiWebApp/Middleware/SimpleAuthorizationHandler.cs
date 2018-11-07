using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ApiWebApp.Middleware
{
    public class SimpleAuthorizationHandler :
        AuthorizationHandler<IsAuthenticatedAuthorizationRequirement, NullResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            IsAuthenticatedAuthorizationRequirement requirement,
            NullResource resource)
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