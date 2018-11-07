using System;
using System.ServiceModel;
using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.Gateways.V2.Arrears.Impl
{
    /// <summary>
    /// ArrearsActionDiaryGateway marshalls calls to the Database for reads and Web Service for writes
    /// </summary>
    public class ArrearsActionDiaryGateway : IArrearsActionDiaryGateway
    {
        /// <summary>
        /// WCF Service Interface which allows us to create action diary entries
        /// </summary>
        private readonly IArrearsAgreementServiceChannel _actionDiaryService;
        public ArrearsActionDiaryGateway(IArrearsAgreementServiceChannel actionDiaryService)
        {
            _actionDiaryService = actionDiaryService;
        }

        public async Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request is null");
            var response = await _actionDiaryService.CreateArrearsActionAsync(request).ConfigureAwait(false);
            if (_actionDiaryService.State != CommunicationState.Closed)
                _actionDiaryService.Close();
            return response;
        }
    }
}
