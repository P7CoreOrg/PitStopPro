using System.Net.Http;

namespace GraphQLPlay.IdentityModelExtras
{
    public interface IDefaultHttpClientFactory
    {
        HttpMessageHandler HttpMessageHandler { get; }
        HttpClient HttpClient { get; }
    }
}