﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GQL.GraphQLCore.Extensions;
using GQL.GraphQLCore.Stores;
using System;


namespace GQL.Rollup.Extensions
{
    public static class AspNetCoreExtensions
    {
        public interface IGraphQLRollupRegistrations
        {
            void AddGraphQLFieldAuthority(IServiceCollection services);
            void AddGraphQLApis(IServiceCollection services);
        }
        public static IServiceCollection AddGraphQLPlayRollup(
            this IServiceCollection services,
            IGraphQLRollupRegistrations graphQLRollupRegistrations)
        {
            services.AddGraphQLCoreTypes();
            graphQLRollupRegistrations.AddGraphQLFieldAuthority(services);
            graphQLRollupRegistrations.AddGraphQLApis(services);
            return services;
        }
        public static IServiceCollection AddGraphQLPlayRollupInMemoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IGraphQLFieldAuthority, InMemoryGraphQLFieldAuthority>();
            services.RegisterGraphQLCoreConfigurationServices(configuration);
            return services;
        }
    }
}
