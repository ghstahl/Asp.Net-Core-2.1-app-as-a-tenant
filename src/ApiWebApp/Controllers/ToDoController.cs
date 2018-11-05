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
    public class TodoItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private IActionContextAccessor _actionContextAccessor;
        private ILogger _logger;
        public ToDoController(IActionContextAccessor actionContextAccessor, ILogger<FormController> logger)
        {
            _actionContextAccessor = actionContextAccessor;
            _logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult<object>> Create(TodoItem item)
        {
            var output = new Dictionary<string, object>();

            _logger.LogInformation("Summary Executing...");
            var request = _actionContextAccessor.ActionContext.HttpContext.Request;
            var host = _actionContextAccessor.ActionContext.HttpContext.Request.Host;
            _logger.LogInformation($"host.value:{host.Value} host.Host:{host.Host} host.HasValue:{host.HasValue} host.Port:{host.Port}");
            _logger.LogInformation(host.ToUriComponent());


            output.Add("authority", $"{request.Scheme}://{request.Host.Value}");
            item.Id = Guid.NewGuid().ToString();
            output.Add("item", item);
            return output;
        }
    }
}