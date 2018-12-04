using System.Collections.Generic;
using System.Threading.Tasks;
using ApiWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiWebApp.Controllers
{
    [Route("")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private IActionContextAccessor _actionContextAccessor;
        private ILogger _logger;
        private IConfiguration _configuration;
        private ISingletonAutoObjectCache<SummaryController, Dictionary<string, object>> _objectCache;

        public SummaryController(
            ISingletonAutoObjectCache<SummaryController, Dictionary<string, object>> objectCache,
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor, ILogger<SummaryController> logger)
        {
            _objectCache = objectCache;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _logger = logger;
           
        }

        Dictionary<string, object> GetOutput()
        {

            var dictionaryCache = _objectCache.Value;

            if (dictionaryCache.TryGetValue("summary-output", out var result))
            {
                return result as Dictionary<string, object>;
            }

            var request = _actionContextAccessor.ActionContext.HttpContext.Request;

            var credits = new Dictionary<string, string>()
            {
                {
                    "ASP.NET Core Test Server",
                    "https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.1"
                },

            };
            var summary = new Dictionary<string, object>
            {
                {"TenantName", _configuration["TenantName"]},
                {"version", "1.0"},
                {"application", "AzureApiFunction"},
                {"author", "Herb Stahl"},
                {"credits", credits},
                {"authority", $"{request.Scheme}://{request.Host.Value}" }
            };

            dictionaryCache.TryAdd("summary-output", summary);
            return summary;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IDictionary<string, object>>> GetAsync()
        {
            _logger.LogInformation("Summary Executing...");
            var output = GetOutput();
            return output;
        }   
    }
}