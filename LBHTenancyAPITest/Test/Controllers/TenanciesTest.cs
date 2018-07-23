using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.TestDoubles.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
{
    public partial class TenanciesTest
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

        private static async Task<ObjectResult> GetIndex(IListTenancies listTenanciesUseCase, List<string> tenancyRefs)
        {
            var controller = new TenanciesController(listTenanciesUseCase);
            var result = await controller.Get(tenancyRefs);
            return result as OkObjectResult;
        }

        private static string ResponseJson(ObjectResult response)
        {
            return JsonConvert.SerializeObject(response.Value);
        }
    }
}
