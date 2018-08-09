using AgreementService;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Services;
using LBHTenancyAPITest.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace LBHTenancyAPITest.Test.Services
{
    public class ActionDiaryServiceTest
    {
        [Fact]
        public async Task WhenGivenActionDiaryDetails_ShouldReturnAnActionDiaryCreatedResponse()
        {
            var gateway = new StubTenanciesGateway();
            var actionDiary = Fake.GenerateActionDiary();

            gateway.SetActionDiaryDetails(actionDiary.TenancyRef, actionDiary);
            var listAllActionDiary = new ListAllArrearsActions(gateway);
            var response = listAllActionDiary.Execute(actionDiary.TenancyRef);

            var builder = new ArrearsServiceRequestBuilder(configuration);
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
            var configuration = new NameValueCollection
            {
                {"UHUsername", "HackneyAPI"},
                {"UHPassword", "Hackney1"},
                {"UHSourceSystem", "HackneyAPI"}
            };
            var builder = new ArrearsServiceRequestBuilder(configuration);
            var expectedRequest = builder.BuildArrearsRequest(new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    ActionBalance = 17,
                    ActionCode = "GEN",
                    Comment = "Added by webservice",
                    TenancyAgreementRef = "000017/01"
                }
            });
            
            Assert.Equal(workOrder.postcode, drsOrder.postcode);
            Assert.Equal(workOrder.prop_ref, drsOrder.prop_ref);
            Assert.Equal(workOrder.priority, drsOrder.priority);
        }
    }
}
