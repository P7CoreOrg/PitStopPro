using System.Collections.Generic;
using GQL.Contracts;
using GQL.GraphQLCore.Controllers;
using GQL.GraphQLCore.Validators;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GQL.Host.Core.Controllers
{
    [Route("api/v1/[controller]")]
    public class GraphQLController : GraphQLControllerBase<GraphQLController>
    {
        public GraphQLController(IHttpContextAccessor httpContextAccessor, ILogger<GraphQLController> logger, IDocumentExecuter executer, IDocumentWriter writer, ISchema schema, IEnumerable<IPluginValidationRule> pluginValidationRules, IScopedSummaryLogger scopedSummaryLogger) : base(httpContextAccessor, logger, executer, writer, schema, pluginValidationRules, scopedSummaryLogger)
        {
        }
    }
}