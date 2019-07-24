using System.Collections.Generic;

namespace AuthRequiredDemoGraphQL.Models
{
    public class IdentityModel
    {
        public List<ClaimModel> Claims { get; set; }
    }
}