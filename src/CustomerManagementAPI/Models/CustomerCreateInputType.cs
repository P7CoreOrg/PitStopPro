using GraphQL.Types;

namespace CustomerManagementAPI.Models
{
    public class CustomerCreateInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
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
