using GQL.GraphQLCore;
using Newtonsoft.Json;

namespace GQL.GraphQLCore.Extensions
{
    internal class GraphQLJsonDocumentWriterOptions : IGraphQLJsonDocumentWriterOptions
    {
        public Formatting Formatting { get; set; }
        public JsonSerializerSettings JsonSerializerSettings { get; set; }
    }
}