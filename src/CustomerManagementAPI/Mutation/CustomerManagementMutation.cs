using CustomerManagementAPI.Models;
using CustomerManagementStore.Serivices;
using GQL.GraphQLCore;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerManagementAPI.Mutation
{
    public class CustomerManagementMutation : IMutationFieldRegistration
    {
        private ICustomerManagmentStore _customerManagmentStore;

        public CustomerManagementMutation(ICustomerManagmentStore customerManagmentStore)
        {
            _customerManagmentStore = customerManagmentStore;
        }
        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<CustomerType>(name: "upsertCustomer",
                description: null,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CustomerCreateInputType>> { Name = "customer" }),
                resolve: async context =>
                {

                    var input = context.GetArgument<CustomerCreateInput>("customer");
                    var customer = new CustomerManagementStore.Model.Customer
                    {
                        Id = input.Id,
                        Name = input.Name,
                        Address = input.Address,
                        City = input.City,
                        EmailAddress = input.EmailAddress,
                        PostalCode = input.PostalCode,
                        TelephoneNumber = input.TelephoneNumber
                    };
                    var result = await _customerManagmentStore.UpsertCustomerAsync(customer);
                    return result;
                }
            );
           
        }
    }
}
