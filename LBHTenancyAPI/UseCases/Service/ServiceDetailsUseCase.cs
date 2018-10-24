using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.UseCases.Versioning;

namespace LBHTenancyAPI.UseCases.Service
{
    public class GetServiceDetailsUseCase: IGetServiceDetailsUseCase
    {
        private readonly IGetVersionUseCase _getVersionUseCase;
        private readonly ServiceDetails _serviceDetails;

        public GetServiceDetailsUseCase(IGetVersionUseCase getVersionUseCase, ServiceDetails serviceDetails)
        {
            _getVersionUseCase = getVersionUseCase;
            _serviceDetails = serviceDetails;
        }

        public async Task<GetServiceDetailsResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await _getVersionUseCase.ExecuteAsync(cancellationToken).ConfigureAwait(false);

            if (_serviceDetails.Version == null)
                _serviceDetails.Version = new ServiceDetailVersion();

            _serviceDetails.Version.Version = response.Version.InformationalVersion;

            var serviceDetailsResponse = new GetServiceDetailsResponse
            {
                ServiceDetails = _serviceDetails
            };
            return serviceDetailsResponse;
        }
    }
}
