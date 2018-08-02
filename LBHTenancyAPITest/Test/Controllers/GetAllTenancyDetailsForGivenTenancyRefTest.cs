using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class GetAllTenancyDetailsForGivenTenancyRefTest
    {
        [Fact]
        public async Task WhenGivenTenancyRefThatDoesntExist_Tenancy_ShouldRespondWithNoResults()
        {
            var allTenancyDetails = new TenancyDetailsForRefStub();
            var response = await GetAllTenancyDetailsForTenancyRef(allTenancyDetails, "NotHere");

            Assert.NotNull(response);

            var actualJson = JSONHelper.ResponseJson(response);

            var expectedJson = JsonConvert.SerializeObject
            (
                new Dictionary<string, object> {{"tenancy_details", new List<TenancyDetailsForRef>()}}
            );

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_TenancyDetails_ShouldCallGetAllTenancyDetailsForTenancyRef()
        {
            var allTenancySpy = new TenancyDetailsForRefSpy();
            await GetAllTenancyDetailsForTenancyRef(allTenancySpy, "EXAMPLE/123");

            allTenancySpy.AssertCalledOnce();
            allTenancySpy.AssertCalledWith("EXAMPLE/123");
        }

        [Fact]
        public async Task WhenGivenATenancyRef_TenancyDetail_ShouldRespondWithFormattedJson_Example1()
        {
            var allTenancyDetails = new TenancyDetailsForRefStub();
            allTenancyDetails.AddTenancyDetail("0test/01", new TenancyDetailsForRef.Tenancy
            {
                TenancyRef="0test/01",
                LastActionCode="123456789",
                ArrearsAgreementStatus="Breached",
                LastActionDate="2018-01-03 00:00:00Z",
                CurrentBalance="23.01",
                PrimaryContactName="Rashmi",
                PrimaryContactLongAddress="AquaLand",
                PrimaryContactPostcode="e8 1hh",
                ArrearsAgreements = new List<TenancyDetailsForRef.ArrearsAgreement>
                {
                    new TenancyDetailsForRef.ArrearsAgreement
                    {
                        Amount ="23.01",
                        Breached ="True",
                        ClearBy ="2018-12-03 00:00:00Z",
                        Frequency ="Weekly",
                        StartBalance ="10.00",
                        Startdate ="2018-01-03 00:00:00Z",
                        Status="test"
                    },
                    new TenancyDetailsForRef.ArrearsAgreement
                    {
                        Amount ="24.01",
                        Breached ="False",
                        ClearBy ="2018-12-04 00:00:00Z",
                        Frequency ="Monthly",
                        StartBalance ="11.00",
                        Startdate ="2018-02-03 00:00:00Z",
                        Status="test1"
                     }
                },
                ArrearsActionDiary = new List<TenancyDetailsForRef.ArrearsActionDiaryEntry>
                {
                    new TenancyDetailsForRef.ArrearsActionDiaryEntry
                    {
                        Balance ="22.0",
                        Code ="DEF12",
                        CodeName ="Code name",
                        Comment ="Some Comments",
                        Date ="2018-12-03 00:00:00Z",
                        UniversalHousingUsername ="Rashmi"
                    },
                    new TenancyDetailsForRef.ArrearsActionDiaryEntry
                    {
                        Balance ="23.0",
                        Code ="DEF123",
                        CodeName ="Code name",
                        Comment ="Some Comments",
                        Date ="2018-11-03 00:00:00Z",
                        UniversalHousingUsername ="Rashmi"
                    }
                }
            });

            var response = await GetAllTenancyDetailsForTenancyRef(allTenancyDetails, "0test/01");
            var actualResponse = JSONHelper.ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject(getExpectedJSONExample1());

            Assert.Equal(expectedJson, actualResponse);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_TenancyDetail_ShouldRespondWithFormattedJson_Example2()
        {
            var allTenancyDetails = new TenancyDetailsForRefStub();

            allTenancyDetails.AddTenancyDetail("0test/02", new TenancyDetailsForRef.Tenancy
            {
                TenancyRef="0test/02",
                LastActionCode="987654321",
                ArrearsAgreementStatus="Active",
                LastActionDate="2017-01-03 00:00:00Z",
                CurrentBalance="21.01",
                PrimaryContactName="Vlad",
                PrimaryContactLongAddress="AquaLand123",
                PrimaryContactPostcode="e8 2ii",
                ArrearsAgreements = new List<TenancyDetailsForRef.ArrearsAgreement>
                {
                    new TenancyDetailsForRef.ArrearsAgreement
                    {
                        Amount ="21.00",
                        Breached ="False",
                        ClearBy ="2017-12-03 00:00:00Z",
                        Frequency ="Weekly",
                        StartBalance ="101.00",
                        Startdate ="2016-01-02 00:00:00Z",
                        Status="test2"
                    },
                    new TenancyDetailsForRef.ArrearsAgreement
                    {
                        Amount ="21.33",
                        Breached ="True",
                        ClearBy ="2017-12-09 00:00:00Z",
                        Frequency ="Monthly",
                        StartBalance ="112.00",
                        Startdate ="2016-03-04 00:00:00Z",
                        Status="test3"
                     }
                },
                ArrearsActionDiary = new List<TenancyDetailsForRef.ArrearsActionDiaryEntry>
                {
                    new TenancyDetailsForRef.ArrearsActionDiaryEntry
                    {
                        Balance ="44.11",
                        Code ="XYZ12",
                        CodeName ="Code one",
                        Comment ="Pretty great comment",
                        Date ="2017-12-15 00:00:00Z",
                        UniversalHousingUsername ="Vlad"
                    },
                    new TenancyDetailsForRef.ArrearsActionDiaryEntry
                    {
                        Balance ="44.22",
                        Code ="XYZ123",
                        CodeName ="Code nine",
                        Comment ="Extra great comment",
                        Date ="2017-11-02 00:00:00Z",
                        UniversalHousingUsername ="Vlad"
                    }
                }
            });

            var response = await GetAllTenancyDetailsForTenancyRef(allTenancyDetails, "0test/02");
            var actualResponse = JSONHelper.ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject(getExpectedJSONExample2());

            Assert.Equal(expectedJson, actualResponse);
        }

        private Dictionary<string, object> getExpectedJSONExample1()
        {
            var expectedTenancydetails = new Dictionary<string, object>
            {
                {"current_arrears_agreement_status", "Breached"},
                {"primary_contact_name", "Rashmi"},
                {"primary_contact_long_address", "AquaLand"},
                {"primary_contact_postcode", "e8 1hh"},
            };

            var result = new Dictionary<string, object>
            {
                {
                    "tenancy_details", expectedTenancydetails
                },
                {
                    "latest_action_diary_events", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            {"balance", "22.0"},
                            {"code", "DEF12"},
                            {"code_name", "Code name"},
                            {"date", "2018-12-03 00:00:00Z"},
                            {"comment", "Some Comments"},
                            {"universal_housing_username", "Rashmi"}
                        },
                        new Dictionary<string, object>
                        {
                            {"balance", "23.0"},
                            {"code", "DEF123"},
                            {"code_name", "Code name"},
                            {"date", "2018-11-03 00:00:00Z"},
                            {"comment", "Some Comments"},
                            {"universal_housing_username", "Rashmi"}
                        }
                    }
                },
                {
                    "latest_arrears_agreements", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            {"amount", "23.01"},
                            {"breached", "True"},
                            {"clear_by", "2018-12-03 00:00:00Z"},
                            {"frequency", "Weekly"},
                            {"start_balance", "10.00"},
                            {"start_date", "2018-01-03 00:00:00Z"},
                            {"status", "test"}
                        },
                        new Dictionary<string, object>
                        {
                            {"amount", "24.01"},
                            {"breached", "False"},
                            {"clear_by", "2018-12-04 00:00:00Z"},
                            {"frequency", "Monthly"},
                            {"start_balance", "11.00"},
                            {"start_date", "2018-02-03 00:00:00Z"},
                            {"status", "test1"}
                        }
                    }
                }
            };
            return result;
        }

        private Dictionary<string, object> getExpectedJSONExample2()
        {
            var expectedTenancydetails = new Dictionary<string, object>
            {
                {"current_arrears_agreement_status", "Active"},
                {"primary_contact_name", "Vlad"},
                {"primary_contact_long_address", "AquaLand123"},
                {"primary_contact_postcode", "e8 2ii"},
            };

            var result = new Dictionary<string, object>
            {
                {
                    "tenancy_details", expectedTenancydetails
                },
                {
                    "latest_action_diary_events", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            {"balance", "44.11"},
                            {"code", "XYZ12"},
                            {"code_name", "Code one"},
                            {"date", "2017-12-15 00:00:00Z"},
                            {"comment", "Pretty great comment"},
                            {"universal_housing_username", "Vlad"}
                        },
                        new Dictionary<string, object>
                        {
                            {"balance", "44.22"},
                            {"code", "XYZ123"},
                            {"code_name", "Code nine"},
                            {"date", "2017-11-02 00:00:00Z"},
                            {"comment", "Extra great comment"},
                            {"universal_housing_username", "Vlad"}
                        }
                    }
                },
                {
                    "latest_arrears_agreements", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            {"amount", "21.00"},
                            {"breached", "False"},
                            {"clear_by", "2017-12-03 00:00:00Z"},
                            {"frequency", "Weekly"},
                            {"start_balance", "101.00"},
                            {"start_date", "2016-01-02 00:00:00Z"},
                            {"status", "test2"}
                        },
                        new Dictionary<string, object>
                        {
                            {"amount", "21.33"},
                            {"breached", "True"},
                            {"clear_by", "2017-12-09 00:00:00Z"},
                            {"frequency", "Monthly"},
                            {"start_balance", "112.00"},
                            {"start_date", "2016-03-04 00:00:00Z"},
                            {"status", "test3"}
                        }
                    }
                }
            };

            return result;
        }

        private class TenancyDetailsForRefSpy : ITenancyDetailsForRef
        {
            private readonly List<object> calledWith;

            public TenancyDetailsForRefSpy()
            {
                calledWith = new List<object>();
            }

            public TenancyDetailsForRef.TenancyResponse Execute(string tenancyRef)
            {
                calledWith.Add(tenancyRef);
                return new TenancyDetailsForRef.TenancyResponse {TenancyDetails = new TenancyDetailsForRef.Tenancy()};
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

        private class TenancyDetailsForRefStub : ITenancyDetailsForRef
        {
            private readonly Dictionary<string, TenancyDetailsForRef.Tenancy> stubTenancyDetails;

            public TenancyDetailsForRefStub()
            {
                stubTenancyDetails = new Dictionary<string, TenancyDetailsForRef.Tenancy>();
            }

            public void AddTenancyDetail(string tenancyRef, TenancyDetailsForRef.Tenancy tenancyDetails)
            {
                stubTenancyDetails[tenancyRef] = tenancyDetails;
            }

            public TenancyDetailsForRef.TenancyResponse Execute(string tenancyRef)
            {
                var savedTenancy = new TenancyDetailsForRef.Tenancy();

                if (stubTenancyDetails.ContainsKey(tenancyRef))
                {
                    savedTenancy = stubTenancyDetails[tenancyRef];
                }

                return new TenancyDetailsForRef.TenancyResponse
                {
                    TenancyDetails = savedTenancy
                };
            }
        }

        private static async Task<ObjectResult> GetAllTenancyDetailsForTenancyRef(ITenancyDetailsForRef tenancyDetailsForRefUseCase,
            string tenancyRef)
        {
            var controller = new TenanciesController(null, null, null, tenancyDetailsForRefUseCase);
            var result = await controller.GetTenancyDetails(tenancyRef);
            return result as OkObjectResult;
        }
    }
}
