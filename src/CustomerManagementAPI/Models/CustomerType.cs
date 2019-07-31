using CustomerManagementStore.Model;
using GraphQL.Types;

namespace CustomerManagementAPI.Models
{
    public class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType()
        {
            Name = "customer";
            Field<StringGraphType>("Id", "The id of the customer");
            Field<StringGraphType>("name", "The name of the customer");
            Field<StringGraphType>("address", "The address of the customer");
            Field<StringGraphType>("postalCode", "The postalCode of the customer");
            Field<StringGraphType>("city", "The city of the customer");
            Field<StringGraphType>("telephoneNumber", "The telephoneNumber of the customer");
            Field<StringGraphType>("emailAddress", "The emailAddress of the customer");
        }
    }
}
