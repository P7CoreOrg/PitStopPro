﻿using System.Security.Claims;
using GraphQL.Language.AST;

namespace GQL.GraphQLCore.Validators
{
    public class OptOutGraphQLClaimsAuthorizationCheck : IGraphQLClaimsAuthorizationCheck
    {
        private IAllUsersOptOutGraphQLClaimsPrincipalAuthStore _allUsersOptOutGraphQLClaimsPrincipalAuthStore;

        public OptOutGraphQLClaimsAuthorizationCheck(
            IAllUsersOptOutGraphQLClaimsPrincipalAuthStore allUsersOptOutGraphQLClaimsPrincipalAuthStore)
        {
            _allUsersOptOutGraphQLClaimsPrincipalAuthStore = allUsersOptOutGraphQLClaimsPrincipalAuthStore;
        }

        public bool ShouldDoAuthorizationCheck(ClaimsPrincipal claimsPrincipal, OperationType operationTye, string fieldName)
        {
            var contains = _allUsersOptOutGraphQLClaimsPrincipalAuthStore.Contains(claimsPrincipal,
                operationTye, fieldName);
            return !contains;
        }
    }
}