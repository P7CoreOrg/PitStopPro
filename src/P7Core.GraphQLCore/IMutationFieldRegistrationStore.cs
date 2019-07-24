using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;

namespace P7Core.GraphQLCore
{
    public interface IMutationFieldRegistrationStore
    {
        int Count { get; }
        void AddGraphTypeFields(MutationCore mutationCore);
    }
}

