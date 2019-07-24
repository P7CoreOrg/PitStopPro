using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace GraphQLPlay.IdentityModelExtras
{

    public class DiscoverCacheContainer : IDiscoveryCacheContainer
    {
        private readonly IDefaultHttpClientFactory _defaultHttpClientFactory;
        private WellknownAuthority _wellknownAuthority;
        private DiscoveryCache _discoveryCache { get; set; }
 
        

        public DiscoverCacheContainer(IDefaultHttpClientFactory defaultHttpClientFactory, 
            WellknownAuthority wellknownAuthority)
        {
            _defaultHttpClientFactory = defaultHttpClientFactory;
            _wellknownAuthority = wellknownAuthority;
        }

        public  DiscoveryCache DiscoveryCache
        {
            get
            {
                if (_discoveryCache == null)
                {
                     

                    DiscoveryPolicy discoveryPolicy = new DiscoveryPolicy()
                    {
                        ValidateIssuerName = false,
                        ValidateEndpoints = false,
                    };
                    if (_wellknownAuthority.AdditionalEndpointBaseAddresses != null && _wellknownAuthority.AdditionalEndpointBaseAddresses.Any())
                    {
                        foreach (var additionalEndpointBaseAddress in _wellknownAuthority.AdditionalEndpointBaseAddresses)
                        {
                            discoveryPolicy.AdditionalEndpointBaseAddresses.Add(additionalEndpointBaseAddress);
                        }
                    }
                    _discoveryCache = new DiscoveryCache(
                        _wellknownAuthority.Authority, 
                        _defaultHttpClientFactory.HttpClient,
                        discoveryPolicy);
                }
                return _discoveryCache;
            }
        }
    }

    public class DiscoverCacheContainerFactory
    {
       
        private IDefaultHttpClientFactory _defaultHttpClientFactory;
        private IOAuth2ConfigurationStore _oAuth2ConfigurationStore;
        private Dictionary<string, DiscoverCacheContainer> _oIDCDiscoverCacheContainers;

        private Dictionary<string, DiscoverCacheContainer> OIDCDiscoverCacheContainers
        {
            get
            {
                if (_oIDCDiscoverCacheContainers == null)
                {
                    _oIDCDiscoverCacheContainers = new Dictionary<string, DiscoverCacheContainer>();
                    var authorities = _oAuth2ConfigurationStore.GetWellknownAuthoritiesAsync().GetAwaiter().GetResult();
                    foreach (var record in authorities)
                    {
                        _oIDCDiscoverCacheContainers.Add(record.Scheme,
                            new DiscoverCacheContainer(_defaultHttpClientFactory, record));
                    }
                }

                return _oIDCDiscoverCacheContainers;
            }
        }

        public DiscoverCacheContainerFactory(
            IDefaultHttpClientFactory defaultHttpClientFactory,
            IOAuth2ConfigurationStore oAuth2ConfigurationStore,
            IConfiguration configuration)
        {
            _defaultHttpClientFactory = defaultHttpClientFactory;
            _oAuth2ConfigurationStore = oAuth2ConfigurationStore;
        }


        public Dictionary<string, DiscoverCacheContainer> GetAll()
        {
            return OIDCDiscoverCacheContainers;
        }
        public DiscoverCacheContainer Get(string scheme)
        {
            if (OIDCDiscoverCacheContainers.ContainsKey(scheme))
            {
                return OIDCDiscoverCacheContainers[scheme];
            }
            return null;
        }
    }
}