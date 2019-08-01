using AuthRequiredDemoGraphQL.Extensions;
using CustomerManagementAPI.Extensions;
using CustomerManagementStore.Extensions;
using GQL.GraphQLCore.Extensions;
using GQL.GraphQLCore.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Serilog;
using MassTransitAbstractions.Extensions;
using GQL.GraphQLHost.Core;

namespace AuditlogService
{
    public class Startup : GraphQLRollupStartup<Startup>
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration, ILogger<Startup> logger) :
                   base(env, configuration, logger)
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
            services.AddMassTransitOptions(Configuration.GetSection("MassTransitOptions"));
            services.AddHostedService<MessageQueueService>();

            var configSection = Configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "redis";
            });
            services.AddInMemoryCustomerManagmentStore();


        }
        protected override void OnConfigureStart(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(Configuration)
               .Enrich.WithMachineName()
               .CreateLogger();
        }

        protected override void OnConfigureEnd(IApplicationBuilder app, IHostingEnvironment env)
        {

        }
    }
}
