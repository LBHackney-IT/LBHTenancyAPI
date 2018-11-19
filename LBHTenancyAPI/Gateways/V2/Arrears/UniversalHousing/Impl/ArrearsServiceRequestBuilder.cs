using System;
using AgreementService;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;

namespace LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing.Impl
{
    public class ArrearsServiceRequestBuilder : IArrearsServiceRequestBuilder
    {
        private readonly ICredentialsService _credentialsService;
        public ArrearsServiceRequestBuilder(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }

        public T BuildArrearsRequest<T>(T arrears) where T:WebRequest
        {
            if(arrears == null)
                throw new ArgumentNullException("ArrearsServiceRequestBuilder-BuildArrearsRequest:arrears is null");
            arrears.DirectUser = _credentialsService.GetUhUserCredentials();
            arrears.SourceSystem = _credentialsService.GetUhSourceSystem();
            return arrears;
        }

        public ArrearsActionCreateRequest BuildNewActionDiaryRequest(ActionDiaryRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("ArrearsServiceRequestBuilder-BuildActionDiaryRequest:request is null");
            var newRequest = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    ActionBalance = request.ActionBalance,
                    ActionCategory = request.ActionCategory,
                    ActionCode = request.ActionCode,
                    Comment = request.Comment,
                    TenancyAgreementRef = request.TenancyAgreementRef
                },
                CompanyCode = request.CompanyCode,
                DirectUser = _credentialsService.GetUhUserCredentials(),
                SessionToken = request.SessionToken,
                SourceSystem = _credentialsService.GetUhSourceSystem()
            };
            return newRequest;
        }
    }
}
