using System.Collections.Generic;
using GraphQL.Types;

namespace P7Core.GraphQLCore
{
    public interface IMutationFieldRegistration
    {
        void AddGraphTypeFields(MutationCore mutationCore);
    }
    
}