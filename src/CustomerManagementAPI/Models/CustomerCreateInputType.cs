using GraphQL.Types;

namespace CustomerManagementAPI.Models
{
    public class CustomerCreateInputType : InputObjectGraphType
    {
        public CustomerCreateInputType()
        {
            Name = "CustomerInput";
            Field<NonNullGraphType<StringGraphType>>("id");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<StringGraphType>>("address");
            Field<NonNullGraphType<StringGraphType>>("postalCode");
            Field<NonNullGraphType<StringGraphType>>("city");
            Field<NonNullGraphType<StringGraphType>>("telephoneNumber");
            Field<NonNullGraphType<StringGraphType>>("emailAddress");
        }
    }
}
