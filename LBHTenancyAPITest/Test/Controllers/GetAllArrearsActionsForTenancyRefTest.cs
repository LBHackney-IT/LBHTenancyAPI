using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class GetAllArrearsActionsForTenancyRefTest
    {
        [Fact]
        public async Task WhenGivenTenancyRefThatDoesntExist_ActionDiary_ShouldRespondWithNoResults()
        {
            var allActions = new AllActionsStub();
            var response = await GetArrearsActionsDetails(allActions, "NotHere");
            Assert.NotNull(response);

            var actualJson = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject
            (
                new Dictionary<string, object> {{"arrears_action_diary_events", new List<ListAllArrearsActions.ArrearsActionDiaryEntry>()}}
            );

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_ActionDiary_ShouldCallGetActionDiaryDetails()
        {
            var allActionDiarySpy = new AllActionDiarySpy();
            await GetArrearsActionsDetails(allActionDiarySpy, "EXAMPLE/123");

            allActionDiarySpy.AssertCalledOnce();
            allActionDiarySpy.AssertCalledWith("EXAMPLE/123");
        }

        [Fact]
        public async Task WhenGivenATenancyRef_ActionDiary_ShouldRespondWithFormattedJson()
        {
            var allActions = new AllActionsStub();

            allActions.AddActionDiary("1test/02", new List<ListAllArrearsActions.ArrearsActionDiaryEntry>
            {
                new ListAllArrearsActions.ArrearsActionDiaryEntry
                {
                    Balance = "10.10",
                    Code = "ABC01",
                    CodeName = "Some Code Name",
                    Date = "11/10/1000",
                    Comment = "Something very interesting!",
                    UniversalHousingUsername = "Vlad"
                },
                new ListAllArrearsActions.ArrearsActionDiaryEntry
                {
                    Balance = "11.20",
                    Code = "DEF12",
                    CodeName = "Another Code here",
                    Date = "22/08/2000",
                    Comment = "Something very not interesting!",
                    UniversalHousingUsername = "Vlad"
                }
             });

            var response = await GetArrearsActionsDetails(allActions, "1test/02");

            var first = new Dictionary<string, object>
            {
                {"balance", "10.10"},
                {"code", "ABC01"},
                {"code_name", "Some Code Name"},
                {"date", "11/10/1000"},
                {"comment", "Something very interesting!"},
                {"universal_housing_username", "Vlad"}
            };

            var second = new Dictionary<string, object>
            {
                {"balance", "11.20"},
                {"code", "DEF12"},
                {"code_name", "Another Code here"},
                {"date", "22/08/2000"},
                {"comment", "Something very not interesting!"},
                {"universal_housing_username", "Vlad"}
            };

            var output = new Dictionary<string, object>
            {
                {"arrears_action_diary_events",
                    new List<Dictionary<string, object>>
                    {
                        first,
                        second
                    }

                }
            };
            var actualResponse = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject(output);

            Assert.Equal(expectedJson, actualResponse);
        }

        private static async Task<ObjectResult> GetArrearsActionsDetails(IListAllArrearsActions listActionDiaryUseCase,
            string tenancyRef)
        {
            var controller = new TenanciesController(null, listActionDiaryUseCase, null);
            var result = await controller.GetActionDiaryDetails(tenancyRef);
            return result as OkObjectResult;
        }

        private static string ResponseJson(ObjectResult response)
        {
            return JsonConvert.SerializeObject(response.Value);
        }

        private class AllActionDiarySpy : IListAllArrearsActions
        {
            private readonly List<object> calledWith;

            public AllActionDiarySpy()
            {
                calledWith = new List<object>();
            }

            public ListAllArrearsActions.ArrearsActionDiaryResponse Execute(string tenancyRef)
            {
                calledWith.Add(tenancyRef);
                return new ListAllArrearsActions.ArrearsActionDiaryResponse {ActionDiaryEntries = new List<ListAllArrearsActions.ArrearsActionDiaryEntry>()};
            }

            public void AssertCalledOnce()
            {
                Assert.Single(calledWith);
            }

            public void AssertCalledWith(object expectedArgument)
            {
                Assert.Equal(expectedArgument, calledWith[0]);
            }
        }

        private class AllActionsStub : IListAllArrearsActions
        {
            private readonly Dictionary<string, List<ListAllArrearsActions.ArrearsActionDiaryEntry>> stubActionDiaryDetails;

            public AllActionsStub()
            {
                stubActionDiaryDetails = new Dictionary<string, List<ListAllArrearsActions.ArrearsActionDiaryEntry>>();
            }

            public void AddActionDiary(string tenancyRef, List<ListAllArrearsActions.ArrearsActionDiaryEntry> actionDiary)
            {
                stubActionDiaryDetails[tenancyRef] = actionDiary;
            }

            public ListAllArrearsActions.ArrearsActionDiaryResponse Execute(string tenancyRef)
            {
                var savedActions = new List<ListAllArrearsActions.ArrearsActionDiaryEntry>();

                if (stubActionDiaryDetails.ContainsKey(tenancyRef))
                {
                    savedActions = stubActionDiaryDetails[tenancyRef];
                }

                return new ListAllArrearsActions.ArrearsActionDiaryResponse
                {
                    ActionDiaryEntries = savedActions
                };
            }
        }
    }
}
