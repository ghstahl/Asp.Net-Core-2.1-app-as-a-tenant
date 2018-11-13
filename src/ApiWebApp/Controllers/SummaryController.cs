using System.Collections.Generic;
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
        public SummaryController(
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor, ILogger<SummaryController> logger)
        {
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _logger = logger;
        }
        private static Dictionary<string, object> _output;
        private IConfiguration _configuration;

        private static Dictionary<string, object> Output
        {
            get
            {
                if (_output == null)
                {
                    var credits = new Dictionary<string, string>()
                    {
                        {"ASP.NET Core Test Server", "https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.1"},

                    };
                    _output = new Dictionary<string, object>
                    {
                        {"version", "1.0"},
                        {"application", "AzureApiFunction"},
                        {"author", "Herb Stahl"},
                        {"credits", credits},

                    };
                }
                return _output;
            }
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IDictionary<string, object>> Get()
        {
            _logger.LogInformation("Summary Executing...");
            var request = _actionContextAccessor.ActionContext.HttpContext.Request;
            var host = _actionContextAccessor.ActionContext.HttpContext.Request.Host;
            _logger.LogInformation($"host.value:{host.Value} host.Host:{host.Host} host.HasValue:{host.HasValue} host.Port:{host.Port}");
            _logger.LogInformation(host.ToUriComponent());

            Dictionary<string, object> value = new Dictionary<string, object>();
            foreach (var item in Output)
            {
                value.Add(item.Key,item.Value);
            }
            value.Add("authority", $"{request.Scheme}://{request.Host.Value}");

            return value;
        }

        
    }
}