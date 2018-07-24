using System;
using System.Collections.Generic;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using SQLitePCL;
using Xunit;
using Xunit.Abstractions;

namespace LBHTenancyAPITest.Test.UseCases
{
    public class ListTenanciesTest
    {
        [Fact]
        public void WhenThereAreNoTenancyRefsGiven_ShouldReturnNone()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(gateway);
            var response = listTenancies.Execute(new List<string>());

            Assert.Empty(response.Tenancies);
        }

        [Fact]
        public void WhenGivenNoTenancyRefs_ShouldReturnATenancyResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(gateway);
            var response = listTenancies.Execute(new List<string>());

            Assert.IsType(typeof(ListTenancies.Response), response);
        }

        [Fact]
        public void WhenGivenATenancyRef_ShouldReturnATenancyResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(gateway);
            var response = listTenancies.Execute(new List<string>());

            Assert.IsType(typeof(ListTenancies.Response), response);
        }

        [Fact]
        public void WhenATenancyRefIsGiven_ResponseShouldIncludeDetailsOnThatTenancy_Example1()
        {
            var gateway = new StubTenanciesGateway();
            var tenancy = Fake.GenerateTenancyListItem();

            gateway.SetTenancyListItem(tenancy.TenancyRef, tenancy);

            var listTenancies = new ListTenancies(gateway);
            var actualResponse = listTenancies.Execute(new List<string> {tenancy.TenancyRef});
            var expectedResponse = new ListTenancies.Response
            {
                Tenancies = new List<ListTenancies.ResponseTenancy>
                {
                    new ListTenancies.ResponseTenancy
                    {
                        TenancyRef = tenancy.TenancyRef,
                        LastActionCode = tenancy.LastActionCode,
                        LastActionDate = String.Format("{0:u}", tenancy.LastActionDate),
                        CurrentBalance = tenancy.CurrentBalance.ToString("C"),
                        ArrearsAgreementStatus = tenancy.ArrearsAgreementStatus,
                        PrimaryContactName = tenancy.PrimaryContactName,
                        PrimaryContactShortAddress = tenancy.PrimaryContactShortAddress,
                        PrimaryContactPostcode = tenancy.PrimaryContactPostcode
                    }
                }
            };

            Assert.Equal(expectedResponse.Tenancies, actualResponse.Tenancies);
        }
    }
}
