using System.Net.Http;

namespace GQL.IdentityModelExtras
{
    public interface IDefaultHttpClientFactory
    {
        HttpMessageHandler HttpMessageHandler { get; }
        HttpClient HttpClient { get; }
    }
}