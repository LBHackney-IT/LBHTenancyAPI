using System.Collections.Generic;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helpers;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases
{
    public class AllArrearsActionsForTenancyTest
    {
        [Fact]
        public void WhenGivenATenancyRefThatDoesntExist_ShouldReturnAnEmptyArrearsActionsResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllActions = new AllArrearsActionsForTenancy(gateway);
            var response = listAllActions.Execute("");

            Assert.IsType(typeof(AllArrearsActionsForTenancy.ArrearsActionDiaryResponse), response);
            Assert.Empty(response.ActionDiaryEntries);
        }

        [Fact]
        public void WhenGivenATenancyRef_ShouldReturnAnActionDiaryResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllActions = new AllArrearsActionsForTenancy(gateway);

            var actionDiary = Fake.GenerateActionDiary();
            gateway.SetActionDiaryDetails(actionDiary.TenancyRef, actionDiary);

            var response = listAllActions.Execute(actionDiary.TenancyRef);

            Assert.IsType(typeof(AllArrearsActionsForTenancy.ArrearsActionDiaryResponse), response);
        }

        [Fact]
        public void WhenATenancyRefIsGiven_ResponseShouldIncludeActionDiaryForThatTenancy_Example1()
        {
            var gateway = new StubTenanciesGateway();
            var actionDiary = Fake.GenerateActionDiary();

            gateway.SetActionDiaryDetails(actionDiary.TenancyRef, actionDiary);

            var listAllActionDiary = new AllArrearsActionsForTenancy(gateway);
            var response = listAllActionDiary.Execute(actionDiary.TenancyRef);

            var expectedResponse = new AllArrearsActionsForTenancy.ArrearsActionDiaryResponse
            {
                ActionDiaryEntries = new List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>
                {
                    new AllArrearsActionsForTenancy.ArrearsActionDiaryEntry
                    {
                        Balance = actionDiary.Balance.ToString("C"),
                        Date = string.Format("{0:u}", actionDiary.Date),
                        Code = actionDiary.Code,
                        CodeName = actionDiary.CodeName,
                        Comment = actionDiary.Comment,
                        UniversalHousingUsername = actionDiary.UniversalHousingUsername
                    }
                }
            };
            Assert.Equal(expectedResponse.ActionDiaryEntries, response.ActionDiaryEntries);
        }
    }
}

