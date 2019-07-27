using System.Collections.Generic;
using GraphQL.Types;

namespace GQL.GraphQLCore
{
    public interface IMutationFieldRegistration
    {
        void AddGraphTypeFields(MutationCore mutationCore);
    }

}