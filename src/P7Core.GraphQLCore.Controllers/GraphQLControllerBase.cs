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
using P7Core.GraphQLCore.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GraphQLPlay.Contracts;

namespace P7Core.GraphQLCore.Controllers
{
    [Produces("application/json")]
    public class GraphQLControllerBase<T> : Controller where T : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILogger Logger { get; set; }
        private IDocumentExecuter _executer { get; set; }
        private IDocumentWriter _writer { get; set; }
        private ISchema _schema { get; set; }
        private readonly IDictionary<string, string> _namedQueries;
        private List<IPluginValidationRule> _pluginValidationRules;
        private IScopedSummaryLogger _scopedSummaryLogger;

        public GraphQLControllerBase(
            IHttpContextAccessor httpContextAccessor,
            ILogger<T> logger,
            IDocumentExecuter executer,
            IDocumentWriter writer,
            ISchema schema,
            IEnumerable<IPluginValidationRule> pluginValidationRules,
            IScopedSummaryLogger scopedSummaryLogger)
        {
            _httpContextAccessor = httpContextAccessor;
            Logger = logger;
            _executer = executer;
            _writer = writer;
            _schema = schema;
            _namedQueries = new Dictionary<string, string>
            {

            };
            _pluginValidationRules = pluginValidationRules.ToList();
            _scopedSummaryLogger = scopedSummaryLogger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] string operationName,
            [FromQuery] string query,
            [FromQuery] string variables)
        {
            return await ProcessQueryAsync(new GraphQLQuery
            {
                Query = query,
                OperationName = operationName,
                Variables = variables == null ? new JObject() : JsonConvert.DeserializeObject<JObject>(variables)
            });
        }
        internal async Task<IActionResult> ProcessQueryAsync(GraphQLQuery query)
        {
            try
            {
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
                    _.ValidationRules = _pluginValidationRules.Concat(DocumentValidator.CoreRules());

                }).ConfigureAwait(false);

                var httpResult = result.Errors?.Count > 0
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.OK;
                MemoryStream stream = new MemoryStream();
                await _writer.WriteAsync(stream, result);
                StreamReader reader = new StreamReader(stream);
                stream.Seek(0, SeekOrigin.Begin);
                string json = reader.ReadToEnd();
                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);

                var objectResult = new ObjectResult(obj) { StatusCode = (int)httpResult };

                var summaryLog = string.Join(";", _scopedSummaryLogger.Select(x => x.Key + "=" + x.Value).ToArray());
                Logger.LogCritical(summaryLog);
                return objectResult;

            }
            catch (Exception ex)
            {
                var summaryLog = string.Join(";", _scopedSummaryLogger.Select(x => x.Key + "=" + x.Value).ToArray());
                Logger.LogError(ex, summaryLog);
                return MakeObjectResult("Unable to process request", HttpStatusCode.NotFound);
            }
        }
        public static ObjectResult MakeObjectResult(string msg, HttpStatusCode httpStatusCode)
        {
            var errorResult = new ExecutionResult
            {
                Errors = new ExecutionErrors()
            };
            errorResult.Errors.Add(new ExecutionError(msg));
            return new ObjectResult(errorResult) { StatusCode = (int)httpStatusCode };
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
            return await ProcessQueryAsync(query);
        }

    }
}
