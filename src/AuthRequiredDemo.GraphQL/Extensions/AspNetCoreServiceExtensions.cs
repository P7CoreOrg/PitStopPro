using AuthRequiredDemoGraphQL.Models;
using AuthRequiredDemoGraphQL.Query;
using Microsoft.Extensions.DependencyInjection;
using P7Core.GraphQLCore;


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
