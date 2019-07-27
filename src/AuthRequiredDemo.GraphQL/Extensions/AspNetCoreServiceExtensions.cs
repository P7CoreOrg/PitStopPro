using AuthRequiredDemoGraphQL.Models;
using AuthRequiredDemoGraphQL.Query;
using GQL.GraphQLCore;
using Microsoft.Extensions.DependencyInjection;


namespace AuthRequiredDemoGraphQL.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLAuthRequiredQuery(this IServiceCollection services)
        {

            // AuthRequired Query
            services.AddTransient<IdentityModelType>();
            services.AddTransient<ClaimModelType>();
            services.AddTransient<IQueryFieldRegistration, AuthRequiredQuery>();
        }
    }
}
