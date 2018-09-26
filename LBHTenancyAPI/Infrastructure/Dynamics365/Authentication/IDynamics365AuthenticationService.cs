using System.Threading.Tasks;

namespace LBHTenancyAPI.Infrastructure.Dynamics365.Authentication
{
    public interface IDynamics365AuthenticationService
    {
        Task<string> GetAccessTokenAsync();
    }
}
