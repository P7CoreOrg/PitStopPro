using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphQLPlay.IdentityModelExtras
{
    /*
     "authorities": [
        {
          "scheme": "self",
          "authority": "https://localhost:44371",
          "additionalEndpointBaseAddresses": [
          ]
        },
        {
          "scheme": "p7identityserver4",
          "authority": "https://p7identityserver4.azurewebsites.net",
          "additionalEndpointBaseAddresses": [
          ]
        },
        {
          "scheme": "google",
          "authority": "https://accounts.google.com",
          "additionalEndpointBaseAddresses": [
          ]
        },
        {
          "scheme": "norton",
          "authority": "https://login-int.norton.com/sso/oidc1/token",
          "additionalEndpointBaseAddresses": [
            "https://login-int.norton.com/sso/idp/OIDC",
            "https://login-int.norton.com/sso/oidc1"
          ]
        },
        {
          "scheme": "demoidentityserverio",
          "authority": "https://demo.identityserver.io",
          "additionalEndpointBaseAddresses": [
          ]
        }
      ]
     */
    public class Oauth2Section
    {
        public List<WellknownAuthority> Authorities { get; set; }
    }
    public class InMemoryOAuth2ConfigurationStore : IOAuth2ConfigurationStore
    {
        
        private IConfiguration _configuration;
        private ILogger<InMemoryOAuth2ConfigurationStore> _logger;
        private Oauth2Section _oAuth2Section;

        public InMemoryOAuth2ConfigurationStore(
            IConfiguration configuration,
            ILogger<InMemoryOAuth2ConfigurationStore> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var section = configuration.GetSection("InMemoryOAuth2ConfigurationStore:oauth2");
            _oAuth2Section = new Oauth2Section();
            section.Bind(_oAuth2Section);
        }

        public Task<WellknownAuthority> GetWellknownAuthorityAsync(string scheme)
        {
            var result = from item in _oAuth2Section.Authorities
                where item.Scheme == scheme
                select item;
            return Task.FromResult(result.FirstOrDefault());

        }

        public Task<List<WellknownAuthority>> GetWellknownAuthoritiesAsync()
        {
            return Task.FromResult(_oAuth2Section.Authorities);
        }
    }
}
