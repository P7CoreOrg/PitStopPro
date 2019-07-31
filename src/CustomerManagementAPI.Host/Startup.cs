using AuthRequiredDemoGraphQL.Extensions;
using GQL.GraphQLCore.Extensions;
using GQL.GraphQLCore.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Pitstop.Infrastructure.Messaging.Extensions;

namespace CustomerManagementAPI.Host
{
    public class Startup : GraphQLRollupStartup<Startup>
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration, ILogger<Startup> logger):
            base(env, configuration,logger)
        {
            
        }

        public override void AddGraphQLFieldAuthority(IServiceCollection services)
        {
            services.TryAddSingleton<IGraphQLFieldAuthority, 
                InMemoryGraphQLFieldAuthority>();
            services.RegisterGraphQLCoreConfigurationServices(Configuration);
        }

        public override void AddGraphQLApis(IServiceCollection services)
        {
           
            //  services.AddBurnerGraphQL();
            //   services.AddBurnerGraphQL2();
            services.AddGraphQLAuthRequiredQuery();
        }

        protected override void AddHealthChecks(HealthCheckBuilder checks)
        {
            checks.AddUrlCheck("https://www.google.com");
        }

        protected override void AddAdditionalServices(IServiceCollection services)
        {
            var configSection = Configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];

            services.AddInfrastructureMessaging((options) =>
            {
                options.Host = host;
                options.Username = userName;
                options.Password = password;
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "redis";
            });
            
        }
    }
}
