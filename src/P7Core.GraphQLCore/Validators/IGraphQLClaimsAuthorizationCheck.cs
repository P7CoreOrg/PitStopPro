using System.Security.Claims;
using GraphQL.Language.AST;

namespace GQL.GraphQLCore.Validators
{
    public interface IGraphQLClaimsAuthorizationCheck
    {
        bool ShouldDoAuthorizationCheck(ClaimsPrincipal claimsPrincipal, OperationType operationTye, string fieldName);
    }
}