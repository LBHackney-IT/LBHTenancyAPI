using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Authentication;
using LBHTenancyAPI.Settings.CRM;

namespace LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client.Factory
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

        public async Task<IHttpClient> CreateClientAsync(bool formatForFetchXml)
        {
            var accessToken = await _dynamics365AuthenticationService.GetAccessTokenAsync().ConfigureAwait(false);

            _client = new RestfulHttpClient();
            _client.SetBaseUrl(_configuration.OrganizationUrl);
            _client.AddDefaultHeader("OData-MaxVersion", "4.0");
            _client.AddDefaultHeader("OData-Version", "4.0");
            _client.AddDefaultHeader("Accept", "application/json");
            _client.AddDefaultHeader("Authorization", "Bearer " + accessToken);

            if(formatForFetchXml)
                _client.AddDefaultHeader("Prefer", "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");

            return _client;
        }
    }
}
