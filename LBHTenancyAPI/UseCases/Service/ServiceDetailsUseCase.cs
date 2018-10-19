using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.UseCases.Versioning;

namespace LBHTenancyAPI.UseCases.Service
{
    public class GetServiceDetailsUseCase: IGetServiceDetailsUseCase
    {
        private readonly IGetVersionUseCase _getVersionUseCase;
        public GetServiceDetailsUseCase(IGetVersionUseCase getVersionUseCase)
        {
            _getVersionUseCase = getVersionUseCase;
        }

        public async Task<GetServiceDetailsResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await _getVersionUseCase.ExecuteAsync(cancellationToken).ConfigureAwait(false);
            var serviceDetailsResponse = new GetServiceDetailsResponse
            {
                ServiceDetails = new ServiceDetails
                {
                    Version = new ServiceDetailVersion
                    {
                        Version = response.Version.ToString(),
                    }
                }
            };
            return serviceDetailsResponse;
        }
    }

    public class GetServiceDetailsResponse
    {
        public ServiceDetails ServiceDetails { get; set; }
    }

    public class ServiceDetails
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Organisation { get; set; }
        public ServiceDetailVersion Version { get; set; }
    }

    public class ServiceDetailVersion
    {
        public string Version { get; set; }
        public string GitCommitHash { get; set; }
        public string ApiVersion { get; set; }
    }
}
