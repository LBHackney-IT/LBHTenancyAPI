using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using LBH.Data.Domain;
using LBHTenancyAPI.Controllers.V1;
using LBHTenancyAPI.UseCases.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers.V1
{
    public class TenanciesControllerIndexTest
    {
        private static readonly NullLogger<TenanciesController> _nullLogger = new NullLogger<TenanciesController>();

        [Fact]
        public async Task WhenGivenNoTenancyRefs_Index_ShouldRespondWithNoResults()
        {
            var listTenancies = new ListTenanciesStub();
            var response = await GetIndex(listTenancies, new List<string>());
            Assert.NotNull(response);

            var actualJson = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject
                (
                    new Dictionary<string, object> {{"tenancies", new List<Dictionary<string, object>>()}}
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
            var startDate = new Faker().Date.Past();

            listTenancies.AddTenancyResponse("000001/01", new ListTenancies.ResponseTenancy
            {
                TenancyRef = "000001/01",
                PropertyRef = "prop/01",
                PaymentRef = "123456789",
                StartDate = string.Format("{0:u}", startDate),
                Tenure = "SEC",
                LastActionCode = "CALLED",
                LastActionDate = "2018-01-01 00:00:00Z",
                CurrentBalance = new Currency(10.66m),
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
                                {"prop_ref", "prop/01"},
                                {"payment_ref", "123456789"},
                                {"start_date", string.Format("{0:u}", startDate)},
                                {"tenure", "SEC"},
                                {
                                    "current_balance", new Dictionary<string, object>
                                    {
                                        {"value", 10.66m},
                                        {"currency_code", "GBP"}
                                    }
                                },
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
                PropertyRef = faker.Random.Hash(),
                PaymentRef = faker.Random.Hash(20),
                StartDate = string.Format("{0:u}", faker.Date.Past()),
                Tenure = faker.Random.Word(),
                LastActionCode = faker.Random.Word(),
                LastActionDate = faker.Date.Recent().ToLongDateString(),
                CurrentBalance = new Currency(faker.Finance.Amount()),
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
                                {"prop_ref", expectedTenancyResponse.PropertyRef},
                                {"payment_ref", expectedTenancyResponse.PaymentRef},
                                {"start_date", string.Format("{0:u}", expectedTenancyResponse.StartDate)},
                                {"tenure", expectedTenancyResponse.Tenure},
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
            var controller = new TenanciesController(listTenanciesUseCase, null, null, null, _nullLogger);
            var result = await controller.Get(tenancyRefs);
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

            public ListTenanciesStub()
            {
                stubTenancies = new Dictionary<string, ListTenancies.ResponseTenancy>();
            }

            public void AddTenancyResponse(string tenancyRef, ListTenancies.ResponseTenancy tenancyResponse)
            {
                stubTenancies[tenancyRef] = tenancyResponse;
            }

            public ListTenancies.Response Execute(List<string> tenancyRefs)
            {
                return new ListTenancies.Response
                {
                    Tenancies = tenancyRefs.ConvertAll(tenancyRef => stubTenancies[tenancyRef])
                };
            }

        }
    }
}
