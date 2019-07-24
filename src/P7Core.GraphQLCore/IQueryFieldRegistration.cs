using System.Collections;
using System.Collections.Generic;
using GraphQL.Language.AST;

namespace P7Core.GraphQLCore
{
    public interface IQueryFieldRegistration
    {
        void AddGraphTypeFields(QueryCore queryCore);
    }
}