using GraphQL.Language.AST;

namespace GQL.GraphQLCore.Validators
{
    public interface IGraphQLAuthStore
    {
        bool Contains(OperationType operationType, string fieldName);
    }
}