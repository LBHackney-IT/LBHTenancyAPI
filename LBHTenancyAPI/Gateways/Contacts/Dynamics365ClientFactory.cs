using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.Dynamics365.Authentication;
using LBHTenancyAPI.Settings.CRM;

namespace LBHTenancyAPI.Gateways.Contacts
{
    public class Dynamics365ClientFactory : IDynamics365ClientFactory
    {
        private HttpClient _client;
        private readonly Dynamics365Settings _configuration;
        private readonly IDynamics365AuthenticationService _dynamics365AuthenticationService;

        public Dynamics365ClientFactory(Dynamics365Settings appSettings, IDynamics365AuthenticationService dynamics365AuthenticationService)
        {
            _configuration = appSettings;
            _dynamics365AuthenticationService = dynamics365AuthenticationService;
        }

        public async Task<HttpClient> CreateClientAsync()
        {
            var accessToken = await _dynamics365AuthenticationService.GetAccessTokenAsync().ConfigureAwait(false);

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.OrganizationUrl);
            _client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _client.DefaultRequestHeaders.Add("OData-Version", "4.0");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            return _client;
        }
    }
}