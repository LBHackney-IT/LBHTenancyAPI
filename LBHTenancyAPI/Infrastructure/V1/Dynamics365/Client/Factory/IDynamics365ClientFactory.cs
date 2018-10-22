using System.Threading.Tasks;

namespace LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client.Factory
{
    public interface IDynamics365ClientFactory
    {
        Task<IHttpClient> CreateClientAsync(bool formatForFetchXml = true);
    }
}
