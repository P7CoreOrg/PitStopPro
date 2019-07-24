using GraphQL.Language.AST;

namespace P7Core.GraphQLCore.Validators
{
    public interface IGraphQLAuthorizationCheck
    {
        bool ShouldDoAuthorizationCheck(OperationType operationTye, string fieldName);
    }
}