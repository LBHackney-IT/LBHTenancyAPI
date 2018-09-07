using System;
using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.Gateways
{
    /// <summary>
    /// ArrearsActionDiaryGateway marshalls calls to the Database for reads and Web Service for writes
    /// </summary>
    public class ArrearsActionDiaryGateway : IArrearsActionDiaryGateway
    {
        /// <summary>
        /// WCF Service Interface which allows us to create action diary entries
        /// </summary>
        private readonly IArrearsAgreementService _actionDiaryService;
        public ArrearsActionDiaryGateway(IArrearsAgreementService actionDiaryService)
        {
            _actionDiaryService = actionDiaryService;
        }

        public async Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request is null");
            var response = await _actionDiaryService.CreateArrearsActionAsync(request).ConfigureAwait(false);
            return response;
        }
    }
}
