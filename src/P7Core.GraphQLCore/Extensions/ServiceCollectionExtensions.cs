using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using GraphQLPlay.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using P7Core.GraphQLCore.Stores;
using P7Core.GraphQLCore.Types;
using P7Core.GraphQLCore.Validators;

namespace P7Core.GraphQLCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGraphQLCoreTypes(this IServiceCollection services)
        {
            services.AddScoped<DefaultScopedSummaryLogger>();
            services.AddScoped<IScopedSummaryLogger>(x => x.GetService<DefaultScopedSummaryLogger>());
            services.AddTransient<ISummaryLogger>(x => x.GetService<DefaultScopedSummaryLogger>());
          

            services.AddTransient<IQueryFieldRegistrationStore,QueryFieldRecordRegistrationStore>();
            services.AddTransient<IMutationFieldRegistrationStore, MutationFieldRecordRegistrationStore>();
            services.AddTransient<ISubscriptionFieldRegistrationStore, SubscriptionFieldRecordRegistrationStore>();

            services.AddTransient<IDocumentBuilder, GraphQLDocumentBuilder>();
            services.AddTransient<IDocumentValidator, DocumentValidator>();
            services.AddTransient<IComplexityAnalyzer, ComplexityAnalyzer>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            services.TryAddTransient<IGraphQLJsonDocumentWriterOptions>(
                _ =>
                {
                    var graphQLJsonDocumentWriterOptions = new GraphQLJsonDocumentWriterOptions
                    {
                        Formatting = Formatting.None,
                        JsonSerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                            Converters = new List<JsonConverter>()
                            {
                                new Newtonsoft.Json.Converters.IsoDateTimeConverter()
                            }
                        }
                    };
                    return graphQLJsonDocumentWriterOptions;
                });

            //services.AddSingleton<IDocumentWriter, GraphQLDocumentWriter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddTransient<QueryCore>();
            services.AddTransient<MutationCore>();
            services.AddTransient<SubscriptionCore>();
            
            services.AddTransient<ISchema, SchemaCore>();
            services.TryAddTransient<Func<Type, GraphType>>(
                x =>
                {
                    var context = x.GetService<IServiceProvider>();
                    return t =>
                    {
                        var res = context.GetService(t);
                        return (GraphType)res;
                    };
                });

            services.AddSingleton<IPluginValidationRule, RequiresAuthValidationRule>();
            services.AddSingleton<IGraphQLAuthorizationCheck, OptOutGraphQLAuthorizationCheck>();
            services.AddSingleton<IGraphQLClaimsAuthorizationCheck, OptOutGraphQLClaimsAuthorizationCheck>();

            services.AddTransient<DynamicType>();

            services.AddSingleton<IDependencyResolver>(
                   c => new FuncDependencyResolver(type => c.GetRequiredService(type)));
        }
    }
}