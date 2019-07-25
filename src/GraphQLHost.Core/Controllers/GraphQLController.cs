using System.Collections.Generic;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using GraphQLPlay.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7Core.GraphQLCore.Controllers;
using P7Core.GraphQLCore.Validators;

namespace GraphQLHost.Core.Controllers
{
    [Route("api/v1/[controller]")]
    public class GraphQLController : GraphQLControllerBase<GraphQLController>
    {
        public GraphQLController(IHttpContextAccessor httpContextAccessor, ILogger<GraphQLController> logger, IDocumentExecuter executer, IDocumentWriter writer, ISchema schema, IEnumerable<IPluginValidationRule> pluginValidationRules, IScopedSummaryLogger scopedSummaryLogger) : base(httpContextAccessor, logger, executer, writer, schema, pluginValidationRules, scopedSummaryLogger)
        {
        }
    }
}