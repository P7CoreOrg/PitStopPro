using System;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GQL.GraphQLCore
{
    public class FieldRecord<TSourceType>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public QueryArguments QueryArguments { get; set; }
        public Func<ResolveFieldContext<object>, Task<object>> Resolve { get; set; }
        public string DeprecationReason { get; set; }
    }
}