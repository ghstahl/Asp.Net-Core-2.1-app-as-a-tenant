using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace ApiWebApp
{
    public class RewriteLowerCaseRule : IRule
    {
        public int StatusCode { get; } = (int)HttpStatusCode.MovedPermanently;

        public void ApplyRule(RewriteContext context)
        {
            HttpRequest request = context.HttpContext.Request;
            PathString path = context.HttpContext.Request.Path;
            PathString pathBase = context.HttpContext.Request.PathBase;
            HostString host = context.HttpContext.Request.Host;

            if (path.HasValue && path.Value.Any(char.IsUpper) || host.HasValue && host.Value.Any(char.IsUpper))
            {
                HostString hostLower;
                if (host.Port == null)
                {
                    hostLower = new HostString(host.Host.ToLower());
                }
                else
                {
                    hostLower = new HostString(host.Host.ToLower(),(int)host.Port);
                }
                context.HttpContext.Request.Host = hostLower;

                PathString pathLower = new PathString(path.Value.ToLower());
                context.HttpContext.Request.Path = pathLower;

                PathString pathBaseLower = new PathString(pathBase.Value.ToLower());
                context.HttpContext.Request.PathBase = pathBaseLower;

            }
            else
            {
                context.Result = RuleResult.ContinueRules;
            }
        }
    }
}