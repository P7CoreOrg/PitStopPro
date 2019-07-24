using GraphQL.Types;

namespace AuthRequiredDemoGraphQL.Models
{
    public class IdentityModelType : ObjectGraphType<Models.IdentityModel>
    {
        public IdentityModelType()
        {
            Name = "identity";
            Field<ListGraphType<ClaimModelType>>("claims", "The Claims of the identity");
        }
    }
}