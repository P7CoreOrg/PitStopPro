using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace P7Core.GraphQLCore
{
    public class QueryFieldRecordRegistrationStore : IQueryFieldRegistrationStore
    {
        private List<IQueryFieldRegistration> _fieldRecordRegistrations;

        public QueryFieldRecordRegistrationStore(IEnumerable<IQueryFieldRegistration> fieldRecordRegistrations)
        {
            _fieldRecordRegistrations = fieldRecordRegistrations.ToList();
        }

        public int Count => _fieldRecordRegistrations.Count;

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            foreach (var item in _fieldRecordRegistrations)
            {
                item.AddGraphTypeFields(queryCore);
            }
        }
    }
}
