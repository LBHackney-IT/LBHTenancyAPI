using System.Collections.Specialized;
using AgreementService;
using LBHTenancyAPI.Interfaces;

namespace LBHTenancyAPI.Services
{
    public class ArrearsServiceRequestBuilder : IArrearsServiceRequestBuilder
    {
        private NameValueCollection _configuration;
        public ArrearsServiceRequestBuilder(NameValueCollection configuration)
        {
            _configuration = configuration;
        }

        public ArrearsActionCreateRequest BuildArrearsRequest(ArrearsActionCreateRequest arrears)
        {
            //arrears = new ArrearsActionCreateRequest();
            arrears.DirectUser = GetUserCredentials();
            arrears.SourceSystem = GetUhSourceSystem();
            arrears.ArrearsAction = new ArrearsActionInfo
            {
                //ActionBalance = 17,
                //ActionCode = "GEN",
                //Comment = "Added by webservice",
                //TenancyAgreementRef = "000017/01"
                ActionBalance = arrears.ArrearsAction.ActionBalance,
                ActionCode = arrears.ArrearsAction.ActionCode,
                Comment = arrears.ArrearsAction.Comment,
                TenancyAgreementRef = arrears.ArrearsAction.TenancyAgreementRef
            };
            return arrears;
        }

        private UserCredential GetUserCredentials()
        {
            return new UserCredential
            {
                UserName = _configuration.Get("UHUsername"),
                UserPassword = _configuration.Get("UHPassword")
            };
        }

        private string GetUhSourceSystem()
        {
            return _configuration.Get("UHSourceSystem");
        }
        
    }
}
