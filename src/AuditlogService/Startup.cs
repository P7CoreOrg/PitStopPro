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
using MassTransitAbstractions;
using System;

namespace AuditlogService
{
    public interface IMyRabbitMQReceiveEndpointRegistration : IRabbitMQReceiveEndpointRegistration { }
    public interface IMyRabbitMQContainer : IRabbitMQContainer { }

    public class MyRabbitMQContainer : RabbitMQContainerBase, IMyRabbitMQContainer
    {
    }
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
            services.AddTransient<IMyRabbitMQReceiveEndpointRegistration, OrderServiceEndpointRegistration>();
            services.AddRabbitMQMassTransit<
                  MassTransitOptions,
                  IMyRabbitMQContainer,
                  MyRabbitMQContainer, IMyRabbitMQReceiveEndpointRegistration>();

            services.AddRabbitMqMassTransitOptions<MassTransitOptions>(Configuration.GetSection("MassTransitOptions"));

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
            var loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            var logger = loggerFactory.CreateLogger("GraphQL.Startup");
            logger.LogInformation("LockUp Configure");

        }

        protected override void OnConfigureEnd(IApplicationBuilder app, IHostingEnvironment env)
        {

        }
    }
}
