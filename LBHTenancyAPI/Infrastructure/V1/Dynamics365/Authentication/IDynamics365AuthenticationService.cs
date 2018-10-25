using System.Threading.Tasks;

namespace LBHTenancyAPI.Infrastructure.V1.Dynamics365.Authentication
{
    public interface IDynamics365AuthenticationService
    {
        Task<string> GetAccessTokenAsync();
    }
}
