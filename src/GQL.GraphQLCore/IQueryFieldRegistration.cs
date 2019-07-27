using System.Collections;
using System.Collections.Generic;
using GraphQL.Language.AST;

namespace GQL.GraphQLCore
{
    public interface IQueryFieldRegistration
    {
        void AddGraphTypeFields(QueryCore queryCore);
    }
}