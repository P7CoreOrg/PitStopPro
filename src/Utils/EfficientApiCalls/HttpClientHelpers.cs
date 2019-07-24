using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Utils.Models;

namespace Utils.EfficientApiCalls
{
    [ExcludeFromCodeCoverage]
    public class HttpClientHelpers
    {
        public static async Task<(string content, HttpStatusCode statusCode)> PostBasicAsync<T>(
            HttpClient httpClient,
            string url,
            List<HttpHeader> additionalHeaders,
            T content,
            CancellationToken cancellationToken)
        {
          
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var json = JsonConvert.SerializeObject(content);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;
                    if (additionalHeaders != null)
                    {
                        foreach (var httpHeader in additionalHeaders)
                        {
                            request.Headers.Add(httpHeader.Name, httpHeader.Value);
                        }
                    }
                    using (var response = await httpClient
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        string c = null;
                        if (response.IsSuccessStatusCode)
                        {
                            c = await response.Content.ReadAsStringAsync();
                        }

                        return (c, response.StatusCode);
                    }
                }
            }
        }
        public static async Task<(string content, HttpStatusCode statusCode)> PostStreamAsync<T>(
            HttpClient httpClient,
            string url,
            List<HttpHeader> additionalHeaders,
            T content,
            CancellationToken cancellationToken)
        {

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            using (var httpContent = CreateHttpContent(content))
            {

                request.Content = httpContent;
                if (additionalHeaders != null)
                {
                    foreach (var httpHeader in additionalHeaders)
                    {
                        request.Headers.Add(httpHeader.Name, httpHeader.Value);
                    }
                }

                using (var response = await httpClient
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {
                    string c = null;
                    if (response.IsSuccessStatusCode)
                    {
                        c = await response.Content.ReadAsStringAsync();
                    }

                    return (c, response.StatusCode);
                }
            }

        }

        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
    }
}
