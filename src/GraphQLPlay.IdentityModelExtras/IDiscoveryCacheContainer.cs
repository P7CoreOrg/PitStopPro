using IdentityModel.Client;

namespace GQL.IdentityModelExtras
{
    public interface IDiscoveryCacheContainer
    {
        DiscoveryCache DiscoveryCache { get; }
    }
}