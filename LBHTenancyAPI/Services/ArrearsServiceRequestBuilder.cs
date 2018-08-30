using AgreementService;
using LBHTenancyAPI.Interfaces;

namespace LBHTenancyAPI.Services
{
    public class ArrearsServiceRequestBuilder : IArrearsServiceRequestBuilder
    {
        private readonly ICredentialsService _credentialsService;
        public ArrearsServiceRequestBuilder(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }

        public ArrearsActionCreateRequest BuildArrearsRequest(ArrearsActionCreateRequest arrears)
        {
            arrears.DirectUser = _credentialsService.GetUhUserCredentials();
            arrears.SourceSystem = _credentialsService.GetUhSourceSystem();
            arrears.ArrearsAction = new ArrearsActionInfo
            {
                ActionBalance = arrears.ArrearsAction.ActionBalance,
                ActionCode = arrears.ArrearsAction.ActionCode,
                Comment = arrears.ArrearsAction.Comment,
                TenancyAgreementRef = arrears.ArrearsAction.TenancyAgreementRef
            };
            return arrears;
        }
    }
}
