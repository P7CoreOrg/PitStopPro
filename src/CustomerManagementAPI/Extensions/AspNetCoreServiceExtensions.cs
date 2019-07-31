
using CustomerManagementAPI.Models;
using CustomerManagementAPI.Mutation;
using CustomerManagementAPI.Query;
using GQL.GraphQLCore;
using Microsoft.Extensions.DependencyInjection;


namespace CustomerManagementAPI.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQCustomerManagementAPI(this IServiceCollection services)
        {

            // AuthRequired Query
            services.AddTransient<CustomerType>();
            services.AddTransient<CustomerCreateInputType>();
            services.AddTransient<IQueryFieldRegistration, CustomerManagementQuery>();
            services.AddTransient<IMutationFieldRegistration, CustomerManagementMutation>();
        }
    }
}
