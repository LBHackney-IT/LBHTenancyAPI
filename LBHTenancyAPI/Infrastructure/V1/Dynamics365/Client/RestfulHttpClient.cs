using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client
{
    public class RestfulHttpClient : IHttpClient
    {
        private static readonly HttpClient _client = new HttpClient();

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
