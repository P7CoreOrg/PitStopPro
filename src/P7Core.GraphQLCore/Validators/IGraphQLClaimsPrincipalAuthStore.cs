using System.Security.Claims;
using GraphQL.Language.AST;

namespace GQL.GraphQLCore.Validators
{
    public interface IGraphQLClaimsPrincipalAuthStore
    {
        bool Contains(ClaimsPrincipal claimsPrincipal, OperationType operationType, string fieldName);
    }
}