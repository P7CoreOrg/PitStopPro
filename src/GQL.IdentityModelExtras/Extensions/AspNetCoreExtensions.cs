
using GQL.IdentityModelExtras;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GQL.GraphQLCore.Extensions
{
    public static class AspNetCoreExtensions
    {
        public static IServiceCollection AddInMemoryOAuth2ConfigurationStore(this IServiceCollection services)
        {
            services.AddSingleton<IOAuth2ConfigurationStore, InMemoryOAuth2ConfigurationStore>();
            return services;
        }
        public static IServiceCollection AddDefaultHttpClientFactory(this IServiceCollection services)
        {
            services.TryAddTransient<IDefaultHttpClientFactory, DefaultHttpClientFactory>();
            return services;
        }
    }
}