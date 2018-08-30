using LBHTenancyAPI.Services;
using Xunit;
using System.Collections.Specialized;
using AgreementService;

namespace LBHTenancyAPITest.Test.Services
{
    public class ArrearsServiceRequestBuilderTests
    {
        [Fact]
        public void Return_A_Built_Request_Object()
        {
            var builder = new ArrearsServiceRequestBuilder(new CredentialsService());
            var request = builder.BuildArrearsRequest(new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo()
            });
            Assert.IsType<ArrearsActionCreateRequest>(request);
        }

        [Fact]
        public void WhenGivenActionDiaryDetails_BuildValidArrearsActionDiaryRequest()
        {
            var builder = new ArrearsServiceRequestBuilder(new CredentialsService());
            var request = builder.BuildArrearsRequest(new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    ActionBalance = 17,
                    ActionCode = "GEN",
                    Comment = "Added by webservice",
                    TenancyAgreementRef = "000017/01"
                }
            });
            Assert.Equal(17, request.ArrearsAction.ActionBalance);
            Assert.Equal("GEN", request.ArrearsAction.ActionCode);
            Assert.Equal("Added by webservice", request.ArrearsAction.Comment);
            Assert.Equal("000017/01", request.ArrearsAction.TenancyAgreementRef);
        }
    }
}
