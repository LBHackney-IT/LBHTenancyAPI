using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.Dynamics365.Authentication;
using LBHTenancyAPI.Settings.CRM;

namespace LBHTenancyAPI.Gateways.Contacts
{
    public class Dynamics365ClientFactory : IDynamics365ClientFactory
    {
        private IHttpClient _client;
        private readonly Dynamics365Settings _configuration;
        private readonly IDynamics365AuthenticationService _dynamics365AuthenticationService;

        public Dynamics365ClientFactory(Dynamics365Settings appSettings, IDynamics365AuthenticationService dynamics365AuthenticationService)
        {
            _configuration = appSettings;
            _dynamics365AuthenticationService = dynamics365AuthenticationService;
        }

        public async Task<IHttpClient> CreateClientAsync()
        {
            var accessToken = await _dynamics365AuthenticationService.GetAccessTokenAsync().ConfigureAwait(false);

            _client = new RestfulHttpClient();
            _client.SetBaseUrl(_configuration.OrganizationUrl);
            _client.AddDefaultHeader("OData-MaxVersion", "4.0");
            _client.AddDefaultHeader("OData-Version", "4.0");
            _client.AddDefaultHeader("Accept", "application/json");
            _client.AddDefaultHeader("Authorization", "Bearer " + accessToken);

            return _client;
        }

    }

    public interface IHttpClient
    {
        void AddDefaultHeader(string key, string value);
        void SetBaseUrl(string baseUrl);
        Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken);
    }

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
