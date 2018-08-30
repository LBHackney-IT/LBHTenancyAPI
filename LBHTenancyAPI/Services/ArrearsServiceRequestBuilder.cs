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
            return arrears;
        }
    }
}
