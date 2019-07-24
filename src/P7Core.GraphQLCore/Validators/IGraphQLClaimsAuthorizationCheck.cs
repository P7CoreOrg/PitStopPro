using System.Security.Claims;
using GraphQL.Language.AST;

namespace P7Core.GraphQLCore.Validators
{
    public interface IGraphQLClaimsAuthorizationCheck
    {
        bool ShouldDoAuthorizationCheck(ClaimsPrincipal claimsPrincipal,OperationType operationTye, string fieldName);
    }
}