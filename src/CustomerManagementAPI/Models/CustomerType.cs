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
           
        }
    }
}
