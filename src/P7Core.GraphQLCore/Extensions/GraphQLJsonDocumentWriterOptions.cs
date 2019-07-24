using Newtonsoft.Json;

namespace P7Core.GraphQLCore.Extensions
{
    internal class GraphQLJsonDocumentWriterOptions : IGraphQLJsonDocumentWriterOptions
    {
        public Formatting Formatting { get; set; }
        public JsonSerializerSettings JsonSerializerSettings { get; set; }
    }
}