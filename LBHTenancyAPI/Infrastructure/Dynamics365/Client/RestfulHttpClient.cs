using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Infrastructure.Dynamics365.Client
{
    public class RestfulHttpClient : IHttpClient
    {
        private readonly HttpClient _client;
        public RestfulHttpClient()
        {
            _client = new HttpClient();
        }

        public RestfulHttpClient(HttpMessageHandler messageHandler)
        {
            _client = new HttpClient(messageHandler);
        }

        public void AddDefaultHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key, value);
        }

        public void SetBaseUrl(string baseUrl)
        {
            _client.BaseAddress = new Uri(baseUrl);
        }

        public async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            return response;
        }
    }
}
