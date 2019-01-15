using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.Controllers.V1;
using LBHTenancyAPI.UseCases.V1;
using LBHTenancyAPITest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers.V1
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
                new Dictionary<string, object>
                {
                    {"tenancy_details", new Dictionary<string, object>
                    {
                        {"ref", null},
                        {"prop_ref", null},
                        {"tenure", null},
                        {"rent", 0.0},
                        {"service", 0.0},
                        {"other_charge", 0.0},
                        {"current_arrears_agreement_status", null},
                        {"current_balance", null},
                        {"primary_contact_name", null},
                        {"primary_contact_long_address", null},
                        {"primary_contact_postcode", null}
                    }},
                    {"latest_action_diary_events", new List<Dictionary<string, object>>()},
                    {"latest_arrears_agreements", new List<Dictionary<string, object>>()}
                }
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
            allTenancyDetails.AddTenancyDetail("0test/01", new Tenancy
            {
                TenancyRef = "0test/01",
                PropertyRef = "prop/01",
                Tenure = "SEC",
                Rent = 91.1m,
                Service = 12.0m,
                OtherCharge = 2.0m,
                AgreementStatus = "Breached",
                CurrentBalance = new Currency(-23000.01m),
                PrimaryContactName = "Rashmi",
                PrimaryContactLongAddress = "AquaLand",
                PrimaryContactPostcode = "e8 1hh",
                ArrearsAgreements = new List<ArrearsAgreement>
                {
                    new ArrearsAgreement
                    {
                        Amount = 23.01m,
                        Breached = true,
                        ClearBy = new DateTime(2018, 12, 03, 0, 0, 0),
                        Frequency = "Weekly",
                        StartBalance = 10.00m,
                        Startdate = new DateTime(2018, 01, 03, 0, 0, 0),
                        Status = "test"
                    },
                    new ArrearsAgreement
                    {
                        Amount = 24.01m,
                        Breached = false,
                        ClearBy = new DateTime(2018, 12, 04, 0, 0, 0),
                        Frequency = "Monthly",
                        StartBalance = 11.00m,
                        Startdate = new DateTime(2018, 02, 03, 0, 0, 00),
                        Status = "test1"
                     }
                },
                ArrearsActionDiary = new List<ArrearsActionDiaryEntry>
                {
                    new ArrearsActionDiaryEntry
                    {
                        Balance = 22.0m,
                        Code = "DEF12",
                        Type = "Code name",
                        Comment = "Some Comments",
                        Date = new DateTime(2018, 12, 03, 0, 0, 0),
                        UniversalHousingUsername = "Rashmi"
                    },
                    new ArrearsActionDiaryEntry
                    {
                        Balance = 23.0m,
                        Code = "DEF123",
                        Type = "Code name",
                        Comment = "Some Comments",
                        Date = new DateTime(2018, 12, 03, 0, 0, 0),
                        UniversalHousingUsername = "Rashmi"
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

            allTenancyDetails.AddTenancyDetail("0test/02", new Tenancy
            {
                TenancyRef = "0test/02",
                PropertyRef = "prop/02",
                Tenure = "TEM",
                Rent = 92.1m,
                Service = 13.0m,
                OtherCharge = 3.0m,
                AgreementStatus = "Active",
                CurrentBalance = new Currency(21.01m),
                PrimaryContactName = "Vlad",
                PrimaryContactLongAddress = "AquaLand123",
                PrimaryContactPostcode = "e8 2ii",
                ArrearsAgreements = new List<ArrearsAgreement>
                {
                    new ArrearsAgreement
                    {
                        Amount = 21.00m,
                        Breached = true,
                        ClearBy = new DateTime(2017, 12, 03, 0, 0, 0),
                        Frequency = "Weekly",
                        StartBalance = 101.00m,
                        Startdate = new DateTime(2016, 1, 02, 0, 0, 0),
                        Status = "test2"
                    },
                    new ArrearsAgreement
                    {
                        Amount = 21.33m,
                        Breached = false,
                        ClearBy = new DateTime(2017, 12, 9, 0, 0, 0),
                        Frequency = "Monthly",
                        StartBalance = 112.00m,
                        Startdate = new DateTime(2016, 3, 4, 0, 0, 0),
                        Status = "test3"
                     }
                },
                ArrearsActionDiary = new List<ArrearsActionDiaryEntry>
                {
                    new ArrearsActionDiaryEntry
                    {
                        Balance = 44.11m,
                        Code = "XYZ12",
                        Type = "Code one",
                        Comment = "Pretty great comment",
                        Date = new DateTime(2017, 12, 15, 0, 0, 0),
                        UniversalHousingUsername = "Vlad"
                    },
                    new ArrearsActionDiaryEntry
                    {
                        Balance = 44.22m,
                        Code = "XYZ123",
                        Type = "Code nine",
                        Comment = "Extra great comment",
                        Date = new DateTime(2017, 11, 2, 0, 0, 0),
                        UniversalHousingUsername = "Vlad"
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
                {"ref", "0test/01"},
                {"prop_ref", "prop/01"},
                {"tenure", "SEC"},
                {"rent", 91.1m},
                {"service", 12.0m},
                {"other_charge", 2.0m},
                {"current_arrears_agreement_status", "Breached"},
                {
                    "current_balance", new Dictionary<string, object>
                    {
                        {"value", -23000.01},
                        {"currency_code", "GBP"}
                    }
                },
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
                            {"balance", 22.0m},
                            {"code", "DEF12"},
                            {"type", "Code name"},
                            {"date", "03/12/2018 00:00:00"},
                            {"comment", "Some Comments"},
                            {"universal_housing_username", "Rashmi"}
                        },
                        new Dictionary<string, object>
                        {
                            {"balance", 23.0m},
                            {"code", "DEF123"},
                            {"type", "Code name"},
                            {"date", "03/12/2018 00:00:00"},
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
                            {"amount", 23.01m},
                            {"breached", true},
                            {"clear_by", "2018-12-03T00:00:00"},
                            {"frequency", "Weekly"},
                            {"start_balance", 10.00m},
                            {"start_date", "2018-01-03T00:00:00"},
                            {"status", "test"}
                        },
                        new Dictionary<string, object>
                        {
                            {"amount", 24.01m},
                            {"breached", false},
                            {"clear_by", "2018-12-04T00:00:00"},
                            {"frequency", "Monthly"},
                            {"start_balance", 11.00m},
                            {"start_date", "2018-02-03T00:00:00"},
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
                {"ref", "0test/02"},
                {"prop_ref", "prop/02"},
                {"tenure", "TEM"},
                {"rent", 92.1m},
                {"service", 13.0m},
                {"other_charge", 3.0m},
                {"current_arrears_agreement_status", "Active"},
                {
                    "current_balance", new Dictionary<string, object>
                    {
                        {"value", 21.01},
                        {"currency_code", "GBP"}
                    }
                },
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
                            {"balance", 44.11m},
                            {"code", "XYZ12"},
                            {"type", "Code one"},
                            {"date", "15/12/2017 00:00:00"},
                            {"comment", "Pretty great comment"},
                            {"universal_housing_username", "Vlad"}
                        },
                        new Dictionary<string, object>
                        {
                            {"balance", 44.22m},
                            {"code", "XYZ123"},
                            {"type", "Code nine"},
                            {"date", "02/11/2017 00:00:00"},
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
                            {"amount", 21.00m},
                            {"breached", true},
                            {"clear_by", "2017-12-03T00:00:00"},
                            {"frequency", "Weekly"},
                            {"start_balance", 101.00m},
                            {"start_date", "2016-01-02T00:00:00"},
                            {"status", "test2"}
                        },
                        new Dictionary<string, object>
                        {
                            {"amount", 21.33},
                            {"breached", false},
                            {"clear_by", "2017-12-09 00:00:00Z"},
                            {"frequency", "Monthly"},
                            {"start_balance", 112.00m},
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
                return new TenancyDetailsForRef.TenancyResponse {TenancyDetails = new Tenancy()};
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
            private readonly Dictionary<string, Tenancy> stubTenancyDetails;

            public TenancyDetailsForRefStub()
            {
                stubTenancyDetails = new Dictionary<string, Tenancy>();
            }

            public void AddTenancyDetail(string tenancyRef, Tenancy tenancyDetails)
            {
                stubTenancyDetails[tenancyRef] = tenancyDetails;
            }

            public TenancyDetailsForRef.TenancyResponse Execute(string tenancyRef)
            {
                var savedTenancy = new Tenancy();

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
