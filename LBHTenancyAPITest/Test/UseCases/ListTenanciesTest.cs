using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helper;
using Xunit;

using FluentAssertions;

namespace LBHTenancyAPITest.Test.UseCases
{
    public class ListTenanciesTest
    {
        [Fact]
        public async Task WhenThereAreNoTenancyRefsGiven_ShouldReturnNone()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(gateway);
            var response = await listTenancies.ExecuteAsync(new List<string>());

            Assert.Empty(response.Tenancies);
        }

        [Fact]
        public async Task WhenGivenNoTenancyRefs_ShouldReturnATenancyResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(gateway);
            var response = await listTenancies.ExecuteAsync(new List<string>());

            Assert.IsType(typeof(ListTenancies.Response), response);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_ShouldReturnATenancyResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listTenancies = new ListTenancies(gateway);
            var response = await listTenancies.ExecuteAsync(new List<string>());

            Assert.IsType(typeof(ListTenancies.Response), response);
        }

        [Fact]
        public async Task WhenGivenSomeTenanciesAndSomeVoid_ShouldReturnMatchedTenancies()
        {
            var gateway = new StubTenanciesGateway();
            var tenancy1 = Fake.GenerateTenancyListItem();
            var tenancy2 = Fake.GenerateTenancyListItem();

            gateway.SetTenancyListItem(tenancy1.TenancyRef, tenancy1);
            gateway.SetTenancyListItem(tenancy2.TenancyRef, tenancy2);

            var listTenancies = new ListTenancies(gateway);
            var actualResponse = await listTenancies.ExecuteAsync(new List<string> {tenancy1.TenancyRef, tenancy2.TenancyRef, "FAKE/01"});
            var expectedResponse = new ListTenancies.Response
            {
                Tenancies = new List<ListTenancies.ResponseTenancy>
                {
                    new ListTenancies.ResponseTenancy
                    {
                        TenancyRef = tenancy1.TenancyRef,
                        PropertyRef = tenancy1.PropertyRef,
                        Tenure = tenancy1.Tenure,
                        LatestTenancyAction = new ListTenancies.LatestTenancyAction
                        {
                            LastActionCode = tenancy1.LastActionCode,
                            LastActionDate = tenancy1.LastActionDate.ToString("u")
                        },
                        CurrentBalance = tenancy1.CurrentBalance.ToString("C"),
                        ArrearsAgreementStatus = tenancy1.ArrearsAgreementStatus,
                        PrimaryContact = new ListTenancies.PrimaryContact
                        {
                            PrimaryContactName = tenancy1.PrimaryContactName,
                            PrimaryContactShortAddress = tenancy1.PrimaryContactShortAddress,
                            PrimaryContactPostcode = tenancy1.PrimaryContactPostcode
                        }
                    },
                    new ListTenancies.ResponseTenancy
                    {
                        TenancyRef = tenancy2.TenancyRef,
                        PropertyRef = tenancy2.PropertyRef,
                        Tenure = tenancy2.Tenure,
                        LatestTenancyAction = new ListTenancies.LatestTenancyAction
                        {
                            LastActionCode = tenancy2.LastActionCode,
                            LastActionDate = tenancy2.LastActionDate.ToString("u")
                        },
                        CurrentBalance = tenancy2.CurrentBalance.ToString("C"),
                        ArrearsAgreementStatus = tenancy2.ArrearsAgreementStatus,
                        PrimaryContact = new ListTenancies.PrimaryContact
                        {
                            PrimaryContactName = tenancy2.PrimaryContactName,
                            PrimaryContactShortAddress = tenancy2.PrimaryContactShortAddress,
                            PrimaryContactPostcode = tenancy2.PrimaryContactPostcode
                        }
                    }
                }
            };

            expectedResponse.Tenancies.ElementAt(0).Should().BeEquivalentTo(actualResponse.Tenancies.ElementAt(0));
            expectedResponse.Tenancies.ElementAt(1).Should().BeEquivalentTo(actualResponse.Tenancies.ElementAt(1));
        }

        [Fact]
        public async Task WhenATenancyRefIsGiven_ResponseShouldIncludeDetailsOnThatTenancy_Example1()
        {
            var gateway = new StubTenanciesGateway();
            var tenancy = Fake.GenerateTenancyListItem();

            gateway.SetTenancyListItem(tenancy.TenancyRef, tenancy);

            var listTenancies = new ListTenancies(gateway);
            var actualResponse = await listTenancies.ExecuteAsync(new List<string> {tenancy.TenancyRef});
            var expectedResponse = new ListTenancies.Response
            {
                Tenancies = new List<ListTenancies.ResponseTenancy>
                {
                    new ListTenancies.ResponseTenancy
                    {
                        TenancyRef = tenancy.TenancyRef,
                        PropertyRef = tenancy.PropertyRef,
                        Tenure = tenancy.Tenure,
                        LatestTenancyAction = new ListTenancies.LatestTenancyAction
                        {
                            LastActionCode = tenancy.LastActionCode,
                            LastActionDate = tenancy.LastActionDate.ToString("u")
                        },
                        CurrentBalance = tenancy.CurrentBalance.ToString("C"),
                        ArrearsAgreementStatus = tenancy.ArrearsAgreementStatus,
                        PrimaryContact = new ListTenancies.PrimaryContact
                        {
                            PrimaryContactName = tenancy.PrimaryContactName,
                            PrimaryContactShortAddress = tenancy.PrimaryContactShortAddress,
                            PrimaryContactPostcode = tenancy.PrimaryContactPostcode
                        }
                    }
                }
            };

            expectedResponse.Tenancies.ElementAt(0).Should().BeEquivalentTo(actualResponse.Tenancies.ElementAt(0));
        }
    }
}
