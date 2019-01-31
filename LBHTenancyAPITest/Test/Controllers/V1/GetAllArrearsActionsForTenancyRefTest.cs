using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Controllers.V1;
using LBHTenancyAPI.UseCases.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers.V1
{
    public class GetAllArrearsActionsForTenancyRefTest
    {
        private static readonly NullLogger<TenanciesController> _nullLogger = new NullLogger<TenanciesController>();

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
        public async Task WhenGivenATenancyRef_ActionDiary_ShouldRespondWithFormattedJson_Example1()
        {
            var allActions = new AllActionsStub();

            allActions.AddActionDiary("1test/02", new List<ListAllArrearsActions.ArrearsActionDiaryEntry>
            {
                new ListAllArrearsActions.ArrearsActionDiaryEntry
                {
                    Balance = "10.10",
                    Code = "ABC01",
                    Type = "Some Code Name",
                    Date = "11/10/1000",
                    Comment = "Something very interesting!",
                    UniversalHousingUsername = "Vlad"
                },
                new ListAllArrearsActions.ArrearsActionDiaryEntry
                {
                    Balance = "11.20",
                    Code = "DEF12",
                    Type = "Another Code here",
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
                {"type", "Some Code Name"},
                {"date", "11/10/1000"},
                {"comment", "Something very interesting!"},
                {"universal_housing_username", "Vlad"}
            };

            var second = new Dictionary<string, object>
            {
                {"balance", "11.20"},
                {"code", "DEF12"},
                {"type", "Another Code here"},
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

        [Fact]
        public async Task WhenGivenATenancyRef_ActionDiary_ShouldRespondWithFormattedJson_Example2()
        {
            var allActions = new AllActionsStub();

            allActions.AddActionDiary("testtest/11", new List<ListAllArrearsActions.ArrearsActionDiaryEntry>
            {
                new ListAllArrearsActions.ArrearsActionDiaryEntry
                {
                    Balance = "166.10",
                    Code = "ACODE",
                    Type = "Great Code Name",
                    Date = "12/11/1222",
                    Comment = "A great comment!",
                    UniversalHousingUsername = "Ritchard"
                },
                new ListAllArrearsActions.ArrearsActionDiaryEntry
                {
                    Balance = "-99.00",
                    Code = "CODE2",
                    Type = "Fantastic Code Name",
                    Date = "21/08/1988",
                    Comment = "A somewhat salubrious comment.",
                    UniversalHousingUsername = "Stephen"
                }
             });

            var response = await GetArrearsActionsDetails(allActions, "testtest/11");

            var first = new Dictionary<string, object>
            {
                {"balance", "166.10"},
                {"code", "ACODE"},
                {"type", "Great Code Name"},
                {"date", "12/11/1222"},
                {"comment", "A great comment!"},
                {"universal_housing_username", "Ritchard"}
            };

            var second = new Dictionary<string, object>
            {
                {"balance", "-99.00"},
                {"code", "CODE2"},
                {"type", "Fantastic Code Name"},
                {"date", "21/08/1988"},
                {"comment", "A somewhat salubrious comment."},
                {"universal_housing_username", "Stephen"}
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
            var controller = new TenanciesController(null, listActionDiaryUseCase, null, null, _nullLogger);
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
