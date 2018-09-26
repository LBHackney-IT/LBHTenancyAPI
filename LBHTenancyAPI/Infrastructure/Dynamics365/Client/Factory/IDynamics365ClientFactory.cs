using System.Threading.Tasks;

namespace LBHTenancyAPI.Infrastructure.Dynamics365.Client.Factory
{
    public interface IDynamics365ClientFactory
    {
        Task<IHttpClient> CreateClientAsync(bool formatForFetchXml = true);
    }
}
