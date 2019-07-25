using System.Net.Http;
using GraphQLPlay.IdentityModelExtras;
using Microsoft.AspNetCore.TestHost;

namespace TestServerFixture
{
    public class TestDefaultHttpClientFactory : IDefaultHttpClientFactory
    {
        public HttpMessageHandler HttpMessageHandler { get; set; }
        public HttpClient HttpClient { get; set; }
    }
}