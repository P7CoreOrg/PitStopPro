using IdentityModel.Client;

namespace GraphQLPlay.IdentityModelExtras
{
    public interface IDiscoveryCacheContainer
    {
        DiscoveryCache DiscoveryCache { get; }
    }
}