﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        public async Task WhenGivenATenancyRef_TenancyDetail_ShouldRespondWithFormattedJson()
        {
            var allTenancyDetails = new TenancyDetailsForRefStub();
            allTenancyDetails.AddTenancyDetail("0test/01", new TenancyDetailsForRef.Tenancy()
            {
                TenancyRef="0test/01",
                LastActionCode="123456789",
                ArrearsAgreementStatus="Breached",
                LastActionDate="2018-01-03 00:00:00Z",
                CurrentBalance="23.01",
                PrimaryContactName="Rashmi",
                PrimaryContactLongAddress="AquaLand",
                PrimaryContactPostcode="e8 1hh",
                ArrearsAgreements = new List<TenancyDetailsForRef.ArrearsAgreement>()
                {
                    new TenancyDetailsForRef.ArrearsAgreement()
                    {
                        Amount ="23.01",
                        Breached ="True",
                        ClearBy ="2018-12-03 00:00:00Z",
                        Frequency ="Weekly",
                        StartBalance ="10.00",
                        Startdate ="2018-01-03 00:00:00Z",
                        Status="test"
                    },
                    new TenancyDetailsForRef.ArrearsAgreement()
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
                ArrearsActionDiary = new List<TenancyDetailsForRef.ArrearsActionDiaryEntry>()
                {
                    new TenancyDetailsForRef.ArrearsActionDiaryEntry()
                    {
                        Balance ="22.0",
                        Code ="DEF12",
                        CodeName ="Code name",
                        Comment ="Some Comments",
                        Date ="2018-12-03 00:00:00Z",
                        UniversalHousingUsername ="Rashmi"
                    },
                    new TenancyDetailsForRef.ArrearsActionDiaryEntry()
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
            var expectedJson = JsonConvert.SerializeObject(getExpectedJSON());

            Assert.Equal(expectedJson, actualResponse);
        }

        public Dictionary<string, object> getEmptyJSON()
        {
            var expectedTenancydetails = new Dictionary<string, object>
            {
                {"action_code", null},
                {"agreement_status",null},
                {"last_action_date", null},
                {"primary_contact_name", null},
                {"primary_contact_long_address", null},
                {"primary_contact_postcode", null},
            };
            var result = new Dictionary<string, object>
            {
                {"tenancy_details", expectedTenancydetails},
                {
                    "latest_action_diary", null
                },
                {
                    "latest_arrears_agreements", null
                }
            };
            return result;
        }

        public Dictionary<string, object> getExpectedJSON()
        {
            var expectedTenancydetails = new Dictionary<string, object>
            {
                {"action_code", "123456789"},
                {"agreement_status", "Breached"},
                {"last_action_date", "2018-01-03 00:00:00Z"},
                {"primary_contact_name", "Rashmi"},
                {"primary_contact_long_address", "AquaLand"},
                {"primary_contact_postcode", "e8 1hh"},
            };

            var result = new Dictionary<string, object>
            {
                {"tenancy_details", expectedTenancydetails},
                {
                    "latest_action_diary", new List<Dictionary<string, object>>
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