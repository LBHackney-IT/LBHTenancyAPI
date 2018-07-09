using System;
using System.Collections.Generic;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using SQLitePCL;
using Xunit;
using Xunit.Abstractions;

namespace LBHTenancyAPITest.Test.UseCases
{
    public class ListTenanciesTest
    {
        private ITestOutputHelper output;

        public ListTenanciesTest(ITestOutputHelper output)
        {
            output = output;
        }

        [Fact]
        public void WhenThereAreNoTenancyRefsGiven_ShouldReturnNone()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(tenanciesGateway: gateway);
            var response = listTenancies.Execute(tenancyRefs: new List<string> { });

            Assert.Empty(response.Tenancies);
        }

        [Fact]
        public void WhenGivenNoTenancyRefs_ShouldReturnATenancyResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(tenanciesGateway: gateway);
            var response = listTenancies.Execute(tenancyRefs: new List<string> { });

            Assert.IsType(typeof(ListTenancies.Response), response);
        }

        [Fact]
        public void WhenGivenATenancyRef_ShouldReturnATenancyResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(tenanciesGateway: gateway);
            var response = listTenancies.Execute(tenancyRefs: new List<string> { });

            Assert.IsType(typeof(ListTenancies.Response), response);
        }

        [Fact]
        public void WhenATenancyRefIsGiven_ResponseShouldIncludeDetailsOnThatTenancy_Example1()
        {
            var gateway = new StubTenanciesGateway();
            gateway.SetTenancyListItem("TEST/01", new TenancyListItem()
            {
                TenancyRef = "TEST/01",
                LastActionCode = "ACTION CODE",
                LastActionDate = new DateTime(2018, 1, 1, 0, 0, 0),
                CurrentBalance = 1.11,
                ArrearsAgreementStatus = "CURRENT AGREEMENT STATUS",
                PrimaryContactName = "RICHARD FOSTER",
                PrimaryContactShortAddress = "123 TEST STREET",
                PrimaryContactPostcode = "AB12 C34"
            });


            var listTenancies = new ListTenancies(tenanciesGateway: gateway);
            var actualResponse = listTenancies.Execute(tenancyRefs: new List<string> {"TEST/01"});
            var expectedResponse = new ListTenancies.Response()
            {
                Tenancies = new List<ListTenancies.ResponseTenancy>
                {
                    new ListTenancies.ResponseTenancy()
                    {
                        TenancyRef = "TEST/01",
                        LastActionCode = "ACTION CODE",
                        LastActionDate = "2018-01-01 00:00:00Z",
                        CurrentBalance = "1.11",
                        ArrearsAgreementStatus = "CURRENT AGREEMENT STATUS",
                        PrimaryContactName = "RICHARD FOSTER",
                        PrimaryContactShortAddress = "123 TEST STREET",
                        PrimaryContactPostcode = "AB12 C34"
                    }
                }
            };

            Assert.Equal(expectedResponse.Tenancies, actualResponse.Tenancies);
        }

        [Fact]
        public void WhenATenancyRefIsGiven_ResponseShouldIncludeDetailsOnThatTenancy_Example2()
        {
            var gateway = new StubTenanciesGateway();
            gateway.SetTenancyListItem("TEST/02", new TenancyListItem()
            {
                TenancyRef = "TEST/02",
                LastActionCode = "OTHER ACTION CODE",
                LastActionDate = new DateTime(2010, 2, 2, 2, 2, 2),
                CurrentBalance = 10.99,
                ArrearsAgreementStatus = "COOL AGREEMENT STATUS",
                PrimaryContactName = "ROBERT FOSTER",
                PrimaryContactShortAddress = "456 TEST STREET",
                PrimaryContactPostcode = "CD34 E56"
            });

            var listTenancies = new ListTenancies(tenanciesGateway: gateway);
            var actualResponse = listTenancies.Execute(tenancyRefs: new List<string> {"TEST/02"});
            var expectedResponse = new ListTenancies.Response()
            {
                Tenancies = new List<ListTenancies.ResponseTenancy>
                {
                    new ListTenancies.ResponseTenancy()
                    {
                        TenancyRef = "TEST/02",
                        LastActionCode = "OTHER ACTION CODE",
                        LastActionDate = "2010-02-02 02:02:02Z",
                        CurrentBalance = "10.99",
                        ArrearsAgreementStatus = "COOL AGREEMENT STATUS",
                        PrimaryContactName = "ROBERT FOSTER",
                        PrimaryContactShortAddress = "456 TEST STREET",
                        PrimaryContactPostcode = "CD34 E56"
                    }
                }
            };

            Assert.Equal(expectedResponse.Tenancies, actualResponse.Tenancies);
        }
    }
}
