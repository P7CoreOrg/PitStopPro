using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthRequiredDemoGraphQL.Models;
using GraphQL;
using GraphQL.Types;
using P7Core.GraphQLCore;

namespace AuthRequiredDemoGraphQL.Query
{
    public class AuthRequiredQuery : IQueryFieldRegistration
    {
        public AuthRequiredQuery()
        {
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.Field<IdentityModelType>(name: "authRequired",
                description: null,
                resolve: Resolver,
                deprecationReason: null);
        }

        private object Resolver(ResolveFieldContext<object> context)
        {
            var userContext = context.UserContext.As<GraphQLUserContext>();
            var result = new Models.IdentityModel { Claims = new List<ClaimModel>() };
            foreach (var claim in userContext.HttpContextAccessor.HttpContext.User.Claims)
            {
                result.Claims.Add(new ClaimModel()
                {
                    Name = claim.Type,
                    Value = claim.Value
                });
            }

            return result;
        }
    }
}
