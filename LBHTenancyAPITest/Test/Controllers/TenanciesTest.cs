using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class TenanciesTest
    {
        [Fact]
        public async Task WhenGivenNoTenancyRefs_Index_ShouldRespondWithNoResults()
        {
            var listTenancies = new ListTenanciesStub();
            var response = await GetIndex(listTenancies, new List<string>());
            Assert.NotNull(response);

            var actualJson = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject(
                new Dictionary<string, object>() {{"tenancies", new List<Dictionary<string, object>>()}}
            );

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_Index_ShouldCallListTenancies()
        {
            var listTenanciesSpy = new ListTenanciesSpy();
            await GetIndex(listTenanciesSpy, new List<string> {"EXAMPLE/123"});

            listTenanciesSpy.AssertCalledOnce();
            listTenanciesSpy.AssertCalledWith(new List<object> {new List<string> {"EXAMPLE/123"}});
        }

        [Fact]
        public async Task WhenGivenATenancyRef_Index_ShouldRespondWithFormattedJson()
        {
            var listTenancies = new ListTenanciesStub();
            listTenancies.AddTenancyResponse("000001/01", new ListTenancies.ResponseTenancy
            {
                TenancyRef = "000001/01",
                LastActionCode = "CALLED",
                LastActionDate = "2018-01-01 00:00:00Z",
                CurrentBalance = "10.66",
                ArrearsAgreementStatus = "ACTIVE",
                PrimaryContactName = "Steven Leighton",
                PrimaryContactShortAddress = "123 Test Street",
                PrimaryContactPostcode = "AB12 C12"
            });

            var response = await GetIndex(listTenancies, new List<string> {"000001/01"});
            var actualJson = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject(
                new Dictionary<string, object>
                {
                    {
                        "tenancies", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                {"ref", "000001/01"},
                                {"current_balance", "10.66"},
                                {"current_arrears_agreement_status", "ACTIVE"},
                                {
                                    "latest_action", new Dictionary<string, string>
                                    {
                                        {"code", "CALLED"},
                                        {"date", "2018-01-01 00:00:00Z"}
                                    }
                                },
                                {
                                    "primary_contact", new Dictionary<string, string>
                                    {
                                        {"name", "Steven Leighton"},
                                        {"short_address", "123 Test Street"},
                                        {"postcode", "AB12 C12"}
                                    }
                                }
                            }
                        }
                    }
                }
            );

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_Index_ShouldRespondWithTenancyInfoForThatTenancy()
        {
            var faker = new Faker();
            var expectedTenancyResponse = new ListTenancies.ResponseTenancy
            {
                TenancyRef = faker.Random.Hash(),
                LastActionCode = faker.Random.Word(),
                LastActionDate = faker.Date.Recent().ToLongDateString(),
                CurrentBalance = faker.Finance.Amount().ToString(),
                ArrearsAgreementStatus = faker.Random.Word(),
                PrimaryContactName = faker.Person.FullName,
                PrimaryContactShortAddress = faker.Address.StreetAddress(),
                PrimaryContactPostcode = faker.Address.ZipCode()
            };

            var listTenancies = new ListTenanciesStub();
            listTenancies.AddTenancyResponse(expectedTenancyResponse.TenancyRef, expectedTenancyResponse);

            var response = await GetIndex(listTenancies, new List<string> {expectedTenancyResponse.TenancyRef});
            var actualJson = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject(
                new Dictionary<string, object>
                {
                    {
                        "tenancies", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                {"ref", expectedTenancyResponse.TenancyRef},
                                {"current_balance", expectedTenancyResponse.CurrentBalance},
                                {"current_arrears_agreement_status", expectedTenancyResponse.ArrearsAgreementStatus},
                                {
                                    "latest_action", new Dictionary<string, string>
                                    {
                                        {"code", expectedTenancyResponse.LastActionCode},
                                        {"date", expectedTenancyResponse.LastActionDate}
                                    }
                                },
                                {
                                    "primary_contact", new Dictionary<string, string>
                                    {
                                        {"name", expectedTenancyResponse.PrimaryContactName},
                                        {"short_address", expectedTenancyResponse.PrimaryContactShortAddress},
                                        {"postcode", expectedTenancyResponse.PrimaryContactPostcode}
                                    }
                                }
                            }
                        }
                    }
                }
            );

            Assert.Equal(expectedJson, actualJson);
        }


        [Fact]
        public async Task WhenGivenATenancyRef_Index_ShouldRespondWithTenancyActionDiaryInfoForThatTenancy()
        {
            var faker = new Faker();
            var expectedTenancyResponse = new ListTenancies.ResponseArrearsActionDiary()
            {
                TenancyRef = faker.Random.Hash(),
                ActionCode = faker.Random.Hash(3),
                ActionDate = new DateTime(faker.Random.Int(1900, 1999), faker.Random.Int(1, 12), faker.Random.Int(1, 28), 9, 30, 0),
                ActionCodeName = faker.Random.Words(),
                ActionComment = faker.Random.Words(),
                ActionBalance = faker.Finance.Amount(),
                UniversalHousingUsername = faker.Name.FullName()
            };

            var listTenancies = new ListTenanciesStub();
            listTenancies.AddActionDiaryResponse(expectedTenancyResponse.TenancyRef, expectedTenancyResponse);

            var response = await GetIndex(listTenancies, expectedTenancyResponse.TenancyRef);
            var actualJson = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject(
                new Dictionary<string, object>
                {
                    {
                        "tenancies", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                {"ref", expectedTenancyResponse.TenancyRef},
                                {"action_balance", expectedTenancyResponse.ActionBalance},
                                {"universal_housing_username", expectedTenancyResponse.UniversalHousingUsername},
                                {
                                    "latest_action", new Dictionary<string, string>
                                    {
                                        {"code", expectedTenancyResponse.ActionCode},
                                        {"code_name", expectedTenancyResponse.ActionCodeName},
                                        {"date", expectedTenancyResponse.ActionDate.ToString()},
                                        {"comment", expectedTenancyResponse.ActionComment}
                                    }
                                }

                            }
                        }
                    }
                }
            );

            Assert.Equal(expectedJson, actualJson);
        }

        private static async Task<ObjectResult> GetIndex(IListTenancies listTenanciesUseCase, List<string> tenancyRefs)
        {
            var controller = new TenanciesController(listTenanciesUseCase);
            var result = await controller.Get(tenancyRefs);
            return result as OkObjectResult;
        }

        private static async Task<ObjectResult> GetIndex(IListTenancies listTenanciesUseCase, string tenancyRef)
        {
            var controller = new TenanciesController(listTenanciesUseCase);
            var result = await controller.GetActionDiaryDetails(tenancyRef);
            return result as OkObjectResult;
        }

        private static string ResponseJson(ObjectResult response)
        {
            return JsonConvert.SerializeObject(response.Value);
        }

        private class ListTenanciesSpy : IListTenancies
        {
            private readonly List<object> calledWith;

            public ListTenanciesSpy()
            {
                calledWith = new List<object>();
            }

            public ListTenancies.Response Execute(List<string> tenancyRefs)
            {
                calledWith.Add(new List<object> {tenancyRefs});
                return new ListTenancies.Response {Tenancies = new List<ListTenancies.ResponseTenancy>()};
            }


            public ListTenancies.ArrearsActionDiaryResponse ExecuteQuery(string tenancyRef)
            {
                calledWith.Add(tenancyRef);
                return new ListTenancies.ArrearsActionDiaryResponse() {ActionDiary = new List<ListTenancies.ResponseArrearsActionDiary>()};
            }

            public void AssertCalledOnce()
            {
                Assert.Single(calledWith);
            }

            public void AssertCalledWith(List<object> expectedArguments)
            {
                Assert.Equal(expectedArguments, calledWith[0]);
            }
        }

        private class ListTenanciesStub : IListTenancies
        {
            private readonly Dictionary<string, ListTenancies.ResponseTenancy> stubTenancies;
            private readonly Dictionary<string, ListTenancies.ResponseArrearsActionDiary> stubActionDiaryDetails;

            public ListTenanciesStub()
            {
                stubTenancies = new Dictionary<string, ListTenancies.ResponseTenancy>();
                stubActionDiaryDetails = new Dictionary<string, ListTenancies.ResponseArrearsActionDiary>();
            }

            public void AddTenancyResponse(string tenancyRef, ListTenancies.ResponseTenancy tenancyResponse)
            {
                stubTenancies[tenancyRef] = tenancyResponse;
            }

            public void AddActionDiaryResponse(string tenancyRef, ListTenancies.ResponseArrearsActionDiary actionDiaryResponse)
            {
                stubActionDiaryDetails[tenancyRef] = actionDiaryResponse;
            }

            public ListTenancies.Response Execute(List<string> tenancyRefs)
            {
                return new ListTenancies.Response
                {
                    Tenancies = tenancyRefs.ConvertAll(tenancyRef => stubTenancies[tenancyRef])
                };
            }

           public ListTenancies.ArrearsActionDiaryResponse ExecuteQuery(string tenancyRef)
           {
                  return new ListTenancies.ArrearsActionDiaryResponse()
                  {
                    //  ActionDiary = tenancyRef.Contains(tenancyRefList => stubTenancies[tenancyRef])
                  };

              //var ActionDiary = new ListTenancies.ArrearsActionDiaryResponse();
              //return ActionDiary;
           }
        }
    }
}
