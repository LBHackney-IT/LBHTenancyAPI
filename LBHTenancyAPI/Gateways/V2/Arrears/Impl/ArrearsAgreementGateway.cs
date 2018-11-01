using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;

namespace LBHTenancyAPI.Gateways.V2.Arrears.Impl
{
    /// <summary>
    /// ArrearsActionDiaryGateway marshalls calls to the Database for reads and Web Service for writes
    /// </summary>
    public class ArrearsAgreementGateway : IArrearsAgreementGateway
    {
        /// <summary>
        /// WCF Service Interface which allows us to create action diary entries
        /// </summary>
        private readonly IArrearsAgreementServiceChannel _actionDiaryService;
        private readonly IArrearsServiceRequestBuilder _arrearsServiceRequestBuilder;

        public ArrearsAgreementGateway(IArrearsAgreementServiceChannel actionDiaryService, IArrearsServiceRequestBuilder arrearsServiceRequestBuilder)
        {
            _actionDiaryService = actionDiaryService;
            _arrearsServiceRequestBuilder = arrearsServiceRequestBuilder;
        }

        /// <summary>
        /// Creates Arrears Agreement by calling Universal Housing WCF Endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ArrearsAgreementResponse> CreateArrearsAgreementAsync(ArrearsAgreementRequest request, CancellationToken cancellationToken)
        {
            //todo: remove executeWrapper Pattern 
            //todo: refactor to V2.SearchController Gateway pattern
            if (request == null)
                throw new BadRequestException();

            request = _arrearsServiceRequestBuilder.BuildArrearsRequest<ArrearsAgreementRequest>(request);
            var response = await _actionDiaryService.CreateArrearsAgreementAsync(request).ConfigureAwait(false);
            if(_actionDiaryService.State != CommunicationState.Closed)
                _actionDiaryService.Close();
            var executeResponse = response;
            return executeResponse;
        }
    }
}
