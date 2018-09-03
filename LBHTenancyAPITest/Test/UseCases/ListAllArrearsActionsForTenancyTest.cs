using System.Collections.Generic;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helper;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases
{
    public class ListAllArrearsActionsForTenancyTest
    {
        [Fact]
        public void WhenGivenATenancyRefThatDoesntExist_ShouldReturnAnEmptyArrearsActionsResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllActions = new ListAllArrearsActions(gateway);
            var response = listAllActions.Execute("");

            Assert.IsType(typeof(ListAllArrearsActions.ArrearsActionDiaryResponse), response);
            Assert.Empty(response.ActionDiaryEntries);
        }

        [Fact]
        public void WhenGivenATenancyRef_ShouldReturnAnActionDiaryResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllActions = new ListAllArrearsActions(gateway);

            var actionDiary = Fake.GenerateActionDiary();
            gateway.SetActionDiaryDetails(actionDiary.TenancyRef, actionDiary);

            var response = listAllActions.Execute(actionDiary.TenancyRef);

            Assert.IsType(typeof(ListAllArrearsActions.ArrearsActionDiaryResponse), response);
        }

        [Fact]
        public void WhenATenancyRefIsGiven_ResponseShouldIncludeActionDiaryForThatTenancy()
        {
            var gateway = new StubTenanciesGateway();
            var actionDiary = Fake.GenerateActionDiary();

            gateway.SetActionDiaryDetails(actionDiary.TenancyRef, actionDiary);

            var listAllActionDiary = new ListAllArrearsActions(gateway);
            var response = listAllActionDiary.Execute(actionDiary.TenancyRef);

            var expectedResponse = new ListAllArrearsActions.ArrearsActionDiaryResponse
            {
                ActionDiaryEntries = new List<ListAllArrearsActions.ArrearsActionDiaryEntry>
                {
                    new ListAllArrearsActions.ArrearsActionDiaryEntry
                    {
                        Balance = actionDiary.Balance.ToString("C"),
                        Date = string.Format("{0:u}", actionDiary.Date),
                        Code = actionDiary.Code,
                        Type = actionDiary.Type,
                        Comment = actionDiary.Comment,
                        UniversalHousingUsername = actionDiary.UniversalHousingUsername
                    }
                }
            };
            Assert.Equal(expectedResponse.ActionDiaryEntries, response.ActionDiaryEntries);
        }
    }
}

