using System;
using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.Gateways
{
    public class ArrearsActionDiaryGateway : IArrearsActionDiaryGateway
    {
        private readonly IArrearsAgreementService _arrearsAgreementService;

        public ArrearsActionDiaryGateway(IArrearsAgreementService arrearsAgreementService)
        {
            _arrearsAgreementService = arrearsAgreementService;
        }
        public async Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request is null");
            var response = await _arrearsAgreementService.CreateArrearsActionAsync(request).ConfigureAwait(false);
            return response;
        }
    }
}
