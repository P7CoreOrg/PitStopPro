using IdentityModel.Client;

namespace GQL.GraphQLCore
{
    public interface IDiscoveryCacheContainer
    {
        DiscoveryCache DiscoveryCache { get; }
    }
}