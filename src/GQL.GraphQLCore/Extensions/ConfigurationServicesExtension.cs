using System.Security.Claims;
using GQL.GraphQLCore.Models;
using GQL.GraphQLCore.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace GQL.GraphQLCore.Extensions
{
    public static class ConfigurationServicesExtension
    {
        public static void RegisterGraphQLCoreConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GraphQLAuthenticationConfig>(configuration.GetSection(GraphQLAuthenticationConfig.WellKnown_SectionName));
            services.Configure<GraphQLFieldAuthorityConfig>(configuration.GetSection(GraphQLFieldAuthorityConfig.WellKnown_SectionName));
        }
    }
}