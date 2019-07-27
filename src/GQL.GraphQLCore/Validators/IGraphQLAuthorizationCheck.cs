using GraphQL.Language.AST;

namespace GQL.GraphQLCore.Validators
{
    public interface IGraphQLAuthorizationCheck
    {
        bool ShouldDoAuthorizationCheck(OperationType operationTye, string fieldName);
    }
}