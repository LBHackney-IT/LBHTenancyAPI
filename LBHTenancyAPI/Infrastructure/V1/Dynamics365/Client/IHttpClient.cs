using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client
{
    public interface IHttpClient
    {
        void AddDefaultHeader(string key, string value);
        void SetBaseUrl(string baseUrl);
        Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken);
        void Dispose();
    }
}
