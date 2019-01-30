using System;
using System.Collections.Generic;
using System.Linq;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Gateways.V1;
using LBHTenancyAPI.UseCases.V1;
using LBHTenancyAPITest.Helpers;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.V1
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
        public void WhenGivenSomeTenanciesAndSomeVoid_ShouldReturnMatchedTenancies()
        {
            var gateway = new StubTenanciesGateway();
            var tenancy1 = Fake.GenerateTenancyListItem();
            var tenancy2 = Fake.GenerateTenancyListItem();

            gateway.SetTenancyListItem(tenancy1.TenancyRef, tenancy1);
            gateway.SetTenancyListItem(tenancy2.TenancyRef, tenancy2);

            var listTenancies = new ListTenancies(gateway);
            var actualResponse = listTenancies.Execute(new List<string> {tenancy1.TenancyRef, tenancy2.TenancyRef, "FAKE/01"});
            var expectedResponse = new ListTenancies.Response
            {
                Tenancies = new List<ListTenancies.ResponseTenancy>
                {
                    new ListTenancies.ResponseTenancy
                    {
                        TenancyRef = tenancy1.TenancyRef,
                        PropertyRef = tenancy1.PropertyRef,
                        PaymentRef = tenancy1.PaymentRef,
                        StartDate = tenancy1.StartDate,
                        Tenure = tenancy1.Tenure,
                        LastActionCode = tenancy1.LastActionCode,
                        LastActionDate = String.Format("{0:u}", tenancy1.LastActionDate),
                        CurrentBalance = new Currency(tenancy1.CurrentBalance),
                        ArrearsAgreementStatus = tenancy1.ArrearsAgreementStatus,
                        PrimaryContactName = tenancy1.PrimaryContactName,
                        PrimaryContactShortAddress = tenancy1.PrimaryContactShortAddress,
                        PrimaryContactPostcode = tenancy1.PrimaryContactPostcode
                    },
                    new ListTenancies.ResponseTenancy
                    {
                        TenancyRef = tenancy2.TenancyRef,
                        PropertyRef = tenancy2.PropertyRef,
                        PaymentRef = tenancy2.PaymentRef,
                        StartDate = tenancy2.StartDate,
                        Tenure = tenancy2.Tenure,
                        LastActionCode = tenancy2.LastActionCode,
                        LastActionDate = String.Format("{0:u}", tenancy2.LastActionDate),
                        CurrentBalance = new Currency(tenancy2.CurrentBalance),
                        ArrearsAgreementStatus = tenancy2.ArrearsAgreementStatus,
                        PrimaryContactName = tenancy2.PrimaryContactName,
                        PrimaryContactShortAddress = tenancy2.PrimaryContactShortAddress,
                        PrimaryContactPostcode = tenancy2.PrimaryContactPostcode
                    }
                }
            };

            Assert.Equal(expectedResponse.Tenancies, actualResponse.Tenancies);
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
                        PropertyRef = tenancy.PropertyRef,
                        PaymentRef = tenancy.PaymentRef,
                        StartDate = tenancy.StartDate,
                        Tenure = tenancy.Tenure,
                        LastActionCode = tenancy.LastActionCode,
                        LastActionDate = String.Format("{0:u}", tenancy.LastActionDate),
                        CurrentBalance = new Currency(tenancy.CurrentBalance),
                        ArrearsAgreementStatus = tenancy.ArrearsAgreementStatus,
                        PrimaryContactName = tenancy.PrimaryContactName,
                        PrimaryContactShortAddress = tenancy.PrimaryContactShortAddress,
                        PrimaryContactPostcode = tenancy.PrimaryContactPostcode
                    }
                }
            };
            ListTenancies.ResponseTenancy responseTenancy = expectedResponse.Tenancies.First();

            Assert.Equal(expectedResponse.Tenancies.Count, actualResponse.Tenancies.Count);
            Assert.Equal(responseTenancy.CurrentBalance.Value, actualResponse.Tenancies.First().CurrentBalance.Value);
            Assert.Equal(responseTenancy.CurrentBalance, actualResponse.Tenancies.First().CurrentBalance);
            Assert.Equal(responseTenancy.PaymentRef, actualResponse.Tenancies.First().PaymentRef);
            Assert.Equal(responseTenancy.PropertyRef, actualResponse.Tenancies.First().PropertyRef);
            Assert.Equal(responseTenancy.StartDate, actualResponse.Tenancies.First().StartDate);
            Assert.Equal(expectedResponse.Tenancies, actualResponse.Tenancies);
        }
    }
}
