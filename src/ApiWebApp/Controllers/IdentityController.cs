using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers
{
    /*
     * I don't like to see [Authorize] in code, I like to see it configured in.
     * My authorization is being handled by PathAuthorizationPolicyMiddleware 
     * This is authorized by it is done in the appsettings.json, take a look
     */
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return new JsonResult(User.Claims.Select(
                c => new { c.Type, c.Value }));
        }
    }
}