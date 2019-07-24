using System.Threading.Tasks;
using GraphQL.Types;

namespace P7Core.GraphQLCore
{
    public class QueryCore : ObjectGraphType<object>
    {
        private int _count;

        public QueryCore()
        {
            
        }
        public QueryCore(IQueryFieldRegistrationStore fieldStore)
        {
            Name = "query";
            fieldStore.AddGraphTypeFields(this);
            _count = fieldStore.Count;
        }

        public int RegistrationCount => _count;
    }
}

