using System.Security.Claims;
using GraphQL.Language.AST;

namespace P7Core.GraphQLCore.Validators
{
    public interface IGraphQLClaimsPrincipalAuthStore
    {
        bool Contains(ClaimsPrincipal claimsPrincipal,OperationType operationType, string fieldName);
    }
}