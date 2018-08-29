using AgreementService;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Services;
using LBHTenancyAPITest.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace LBHTenancyAPITest.Test.Services
{
    public class ActionDiaryServiceTest
    {
        //[Fact]
        //public async Task WhenGivenActionDiaryDetails_ShouldReturnAnActionDiaryCreatedResponse()
        //{
        //    var gateway = new StubTenanciesGateway();
        //    var actionDiary = Fake.GenerateActionDiaryRequest();

        //    gateway.SetActionDiaryDetails(actionDiary.ArrearsAction.TenancyAgreementRef, actionDiary.ArrearsAction.Comment);

        //    var actionDiaryRecord = new ArrearsActionDiaryService();
        //    var actualResponse = await actionDiaryRecord.CreateActionDiaryRecord(actionDiary);

        //    var expectedResponse = new ArrearsActionCreateRequest
        //    {
        //        ArrearsAction = new ArrearsActionInfo
        //        {
        //            ActionBalance = actionDiary.ArrearsAction.ActionBalance,
        //            ActionCategory = actionDiary.ArrearsAction.ActionCategory,
        //            ActionCode = actionDiary.ArrearsAction.ActionCode,
        //            TenancyAgreementRef = actionDiary.ArrearsAction.TenancyAgreementRef
        //        }
        //    };

        //} 

    }
}

