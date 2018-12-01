﻿using System.Collections.Generic;
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
        private ISingletonDictionaryCache<SummaryController> _dictionaryCache;

        public SummaryController(
            ISingletonDictionaryCache<SummaryController> dictionaryCache,
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor, ILogger<SummaryController> logger)
        {
            _dictionaryCache = dictionaryCache;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _logger = logger;
        }

        Dictionary<string, object> GetOutput()
        {
            if(_dictionaryCache.TryGet("summary-output",out var result))
            {
                return result as Dictionary<string, object>;
            }

                var credits = new Dictionary<string, string>()
                {
                    {"ASP.NET Core Test Server", "https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.1"},

                };
                var summary = new Dictionary<string, object>
                {
                    {"TenantName",_configuration["TenantName"] },
                    {"version", "1.0"},
                    {"application", "AzureApiFunction"},
                    {"author", "Herb Stahl"},
                    {"credits", credits},

                };
            _dictionaryCache.Set("summary-output",summary);
            return summary;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IDictionary<string, object>>> GetAsync()
        {
            _logger.LogInformation("Summary Executing...");
            var request = _actionContextAccessor.ActionContext.HttpContext.Request;
            var host = _actionContextAccessor.ActionContext.HttpContext.Request.Host;
            _logger.LogInformation($"host.value:{host.Value} host.Host:{host.Host} host.HasValue:{host.HasValue} host.Port:{host.Port}");
            _logger.LogInformation(host.ToUriComponent());

            Dictionary<string, object> value = new Dictionary<string, object>();
            foreach (var item in GetOutput())
            {
                value.Add(item.Key,item.Value);
            }
            value.Add("authority", $"{request.Scheme}://{request.Host.Value}");

            return value;
        }   
    }
}