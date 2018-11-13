using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> GetAsync()
        {
            return new JsonResult(User.Claims.Select(
                c => new { c.Type, c.Value }));
        }
    }
}