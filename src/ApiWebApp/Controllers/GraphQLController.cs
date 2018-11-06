using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiWebApp.Controllers
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
    public class GraphQLUserContext
    {
        public IHttpContextAccessor HttpContextAccessor { get; private set; }

        public GraphQLUserContext(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GraphQLController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;
        private ILogger Logger { get; set; }
        private IDocumentExecuter _executer { get; set; }
        private IDocumentWriter _writer { get; set; }
        private ISchema _schema { get; set; }
        private readonly IDictionary<string, string> _namedQueries;

        public GraphQLController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<GraphQLController> logger,
            IDocumentExecuter executer,
            IDocumentWriter writer,
            ISchema schema)
        {
            _httpContextAccessor = httpContextAccessor;
            Logger = logger;
            _executer = executer;
            _writer = writer;
            _schema = schema;
            _namedQueries = new Dictionary<string, string>
            {
                ["a-query"] = @"query foo { hero { name } }"
            };
          
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {

            string body;
            using (var streamReader = new StreamReader(Request.Body))
            {
                body = await streamReader.ReadToEndAsync().ConfigureAwait(true);
            }

            var query = JsonConvert.DeserializeObject<GraphQLQuery>(body);

            var inputs = query.Variables.ToInputs();
            var queryToExecute = query.Query;

            var result = await _executer.ExecuteAsync(_ =>
            {
                _.UserContext = new GraphQLUserContext(_httpContextAccessor);
                _.Schema = _schema;
                _.Query = queryToExecute;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
                _.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };
                _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();
                _.ValidationRules = DocumentValidator.CoreRules();

            }).ConfigureAwait(false);

            var httpResult = result.Errors?.Count > 0
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.OK;

            var json = _writer.Write(result);
            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);

            var rr = new ObjectResult(obj) { StatusCode = (int)httpResult };
            return rr;
        }
    }
}