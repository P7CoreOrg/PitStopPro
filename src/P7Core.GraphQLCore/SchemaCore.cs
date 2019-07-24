using System;
using GraphQL;
using GraphQL.Types;

namespace P7Core.GraphQLCore
{
    public class SchemaCore : Schema
    {
        public SchemaCore(QueryCore query,
            MutationCore mutation,
            SubscriptionCore subscription, 
            IDependencyResolver resolver)
        {
            
            if (query.RegistrationCount > 0)
            {
                Query = query;
            }
            if (mutation.RegistrationCount > 0)
            {
                Mutation = mutation;
            }
            if (subscription.RegistrationCount > 0)
            {
                Subscription = subscription;
            }
            
            DependencyResolver = resolver;
        }
    }
}