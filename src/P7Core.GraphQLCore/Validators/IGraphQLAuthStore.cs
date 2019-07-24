using GraphQL.Language.AST;

namespace P7Core.GraphQLCore.Validators
{
    public interface IGraphQLAuthStore
    {
        bool Contains(OperationType operationType, string fieldName);
    }
}