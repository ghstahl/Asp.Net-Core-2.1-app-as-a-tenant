using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace ApiWebApp.Controllers
{
    public class ErrorResult
    {
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private IActionContextAccessor _actionContextAccessor;
        private ILogger _logger;
        public FormController(IActionContextAccessor actionContextAccessor, ILogger<FormController> logger)
        {
            _actionContextAccessor = actionContextAccessor;
            _logger = logger;
        }
        // Post api/FormPost
        [HttpPost]
        public async Task<ActionResult<object>> Post()
        {
            _logger.LogTrace("Processing form post request.");
            var context = _actionContextAccessor.ActionContext.HttpContext;
            // validate HTTP
            if (!HttpMethods.IsPost(context.Request.Method) || !context.Request.HasFormContentType)
            {
                _logger.LogWarning("Invalid HTTP request for form post endpoint");
                return Error("Invalid HTTP request for form post endpoint");
            }
            var form = (await context.Request.ReadFormAsync()).AsNameValueCollection();
            var output = new Dictionary<string, string>();
           
            foreach (var key in form.AllKeys)
            {
                output.Add(key,form.Get(key));
            }
            _logger.LogInformation("Summary Executing...");
            var request = _actionContextAccessor.ActionContext.HttpContext.Request;
            var host = _actionContextAccessor.ActionContext.HttpContext.Request.Host;
            _logger.LogInformation($"host.value:{host.Value} host.Host:{host.Host} host.HasValue:{host.HasValue} host.Port:{host.Port}");
            _logger.LogInformation(host.ToUriComponent());


            output.Add("authority", $"{request.Scheme}://{request.Host.Value}");

            return output;
        }
        private ErrorResult Error(string error, string errorDescription = null, Dictionary<string, object> custom = null)
        {
            var response = new ErrorResult
            {
                Error = error,
                ErrorDescription = errorDescription,
            };

            return response;
        }
    }
}