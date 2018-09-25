using System.Net.Http;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Gateways.Contacts
{
    public interface IDynamics365ClientFactory
    {
        Task<IHttpClient> CreateClientAsync();
    }





}
