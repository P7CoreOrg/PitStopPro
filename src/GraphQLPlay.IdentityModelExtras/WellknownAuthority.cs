using System.Collections.Generic;

namespace GraphQLPlay.IdentityModelExtras
{
    public class WellknownAuthority
    {
        public string Scheme { get; set; }
        public string Authority { get; set; }
        public List<string> AdditionalEndpointBaseAddresses { get; set; }
    }
}