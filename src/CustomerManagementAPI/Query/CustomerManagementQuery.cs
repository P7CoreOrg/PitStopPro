using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerManagementAPI.Models;
using CustomerManagementStore.Serivices;
using GQL.GraphQLCore;
using GraphQL;
using GraphQL.Types;

namespace CustomerManagementAPI.Query
{
    public class CustomerManagementQuery : IQueryFieldRegistration
    {
        private ICustomerManagmentStore _customerManagmentStore;

        public CustomerManagementQuery(ICustomerManagmentStore customerManagmentStore)
        {
            _customerManagmentStore = customerManagmentStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<CustomerType>(
                  "customer",
                  arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }),
                  resolve: async context => {
                      return await context.TryAsyncResolve(CustomerResolver);
                  }
              );
        }

        private async Task<object> CustomerResolver(ResolveFieldContext<object> context)
        {
            var usercontext = context.UserContext.As<GraphQLUserContext>();
            var id = context.GetArgument<String>("id");
            var customer = await _customerManagmentStore.GetCustomerAsync(id);
            return customer;
        }
    }
}
