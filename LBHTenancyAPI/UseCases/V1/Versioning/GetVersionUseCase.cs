using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LBHTenancyAPI.UseCases.V1.Versioning
{
    public class GetVersionUseCase:IGetVersionUseCase
    {
        public async Task<GetVersionResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var assembly = typeof(Startup)
                .GetTypeInfo()
                .Assembly;


            var response = new GetVersionResponse
            {
                Version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            };
            return response;
        }
    }

    public class GetVersionResponse
    {
        public AssemblyInformationalVersionAttribute Version { get; set; }
    }
}
