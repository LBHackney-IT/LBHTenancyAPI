using System;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways.Search;
using LBHTenancyAPI.UseCases.Contacts;
using LBHTenancyAPI.UseCases.Contacts.Models;
using Moq;
using Xunit;
using System.Threading;
using System.Collections.Generic;
using LBHTenancyAPI.Infrastructure.Exceptions;
using FluentAssertions;
using LBH.Data.Domain;
using LBHTenancyAPI.UseCases.Search;

namespace LBHTenancyAPITest.Test.UseCases.Search
{
    public class SearchTenancyUseCaseTests
    {
        private ISearchTenancyUseCase _classUnderTest;
        private Mock<ISearchGateway> _fakeGateway;

        public SearchTenancyUseCaseTests()
        {
            _fakeGateway = new Mock<ISearchGateway>();

            _classUnderTest = new SearchTenancyUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidedInput__WhenExecuteAsync_GatewayReceivesCorrectInput()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.SearchTerm.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new List<LBH.Data.Domain.TenancyListItem>
                {

                });

            var request = new SearchTenancyRequest
            {
                SearchTerm = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            _fakeGateway.Verify(v => v.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.SearchTerm.Equals("Test")), CancellationToken.None));
        }

        [Fact]
        public async Task GivenNullInput_WhenExecuteAsync_ThenShouldThrowBadRequestException()
        {
            //arrange
            SearchTenancyRequest request = null;
            //act
            //assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task Given_InvalidInput_ThenShouldThrowBadRequestException()
        {
            //arrange
            var request = new SearchTenancyRequest();
            //act
            //assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task GivenValidedInput_WhenGatewayRespondsWithNull_ThenContactsListShouldBeNull()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.SearchTerm.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(null as List<LBH.Data.Domain.TenancyListItem>);

            var request = new SearchTenancyRequest
            {
                SearchTerm = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Tenancies.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidedInput__WhenExecuteAsync_ThenShouldReturnListOfContacts()
        {
            //arrange
            var tenancy1 = new TenancyListItem
            {
                PrimaryContactName = "test",
                TenancyRef = "tRef",
                ArrearsAgreementStartDate = DateTime.Now,
                ArrearsAgreementStatus = "Active",
                CurrentBalance = 0,
                LastActionCode = "ACC",
                LastActionDate = DateTime.Now.AddDays(-1),
                PrimaryContactPostcode = "test",
                PrimaryContactShortAddress = "123DreryLane",
                PropertyRef = "2",
                Tenure = "LongLease"
            };
            var tenancy2 = new TenancyListItem
            {
                PrimaryContactName = "test2",
                TenancyRef = "tRef2",
                ArrearsAgreementStartDate = DateTime.Now,
                ArrearsAgreementStatus = "Active2",
                CurrentBalance = 1,
                LastActionCode = "ACC2",
                LastActionDate = DateTime.Now.AddDays(-2),
                PrimaryContactPostcode = "test2",
                PrimaryContactShortAddress = "123DreryLane2",
                PropertyRef = "22",
                Tenure = "LongLease2"
            };
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.SearchTerm.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new List<TenancyListItem>
                {
                    tenancy1,
                    tenancy2
                });

            var request = new SearchTenancyRequest
            {
                SearchTerm = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Tenancies.Should().NotBeNullOrEmpty();
            response.Tenancies[0].PropertyRef.Should().BeEquivalentTo(tenancy1.PropertyRef);
            response.Tenancies[0].TenancyRef.Should().BeEquivalentTo(tenancy1.TenancyRef);
            response.Tenancies[0].Tenure.Should().BeEquivalentTo(tenancy1.Tenure);
            response.Tenancies[0].CurrentBalance.Should().BeEquivalentTo(tenancy1.CurrentBalance.ToString("C"));
            response.Tenancies[0].PrimaryContact.Name.Should().BeEquivalentTo(tenancy1.PrimaryContactName);
            response.Tenancies[0].PrimaryContact.Postcode.Should().BeEquivalentTo(tenancy1.PrimaryContactPostcode);
            response.Tenancies[0].PrimaryContact.ShortAddress.Should().BeEquivalentTo(tenancy1.PrimaryContactShortAddress);
            response.Tenancies[0].ArrearsAgreementStatus.Should().BeEquivalentTo(tenancy1.ArrearsAgreementStatus);
            response.Tenancies[0].LatestTenancyAction.Should().NotBeNull();
            response.Tenancies[0].LatestTenancyAction.LastActionCode.Should().BeEquivalentTo(tenancy1.LastActionCode);
            response.Tenancies[0].LatestTenancyAction.LastActionDate.Should().BeEquivalentTo(string.Format("{0}:u",tenancy1.LastActionDate));

            response.Tenancies[1].PropertyRef.Should().BeEquivalentTo(tenancy2.PropertyRef);
            response.Tenancies[1].TenancyRef.Should().BeEquivalentTo(tenancy2.TenancyRef);
            response.Tenancies[1].Tenure.Should().BeEquivalentTo(tenancy2.Tenure);
            response.Tenancies[1].CurrentBalance.Should().BeEquivalentTo(tenancy2.CurrentBalance.ToString("C"));
            response.Tenancies[1].PrimaryContact.Name.Should().BeEquivalentTo(tenancy2.PrimaryContactName);
            response.Tenancies[1].PrimaryContact.Postcode.Should().BeEquivalentTo(tenancy2.PrimaryContactPostcode);
            response.Tenancies[1].PrimaryContact.ShortAddress.Should().BeEquivalentTo(tenancy2.PrimaryContactShortAddress);
            response.Tenancies[1].ArrearsAgreementStatus.Should().BeEquivalentTo(tenancy2.ArrearsAgreementStatus);
            response.Tenancies[1].LatestTenancyAction.Should().NotBeNull();
            response.Tenancies[1].LatestTenancyAction.LastActionCode.Should().BeEquivalentTo(tenancy2.LastActionCode);
            response.Tenancies[1].LatestTenancyAction.LastActionDate.Should().BeEquivalentTo(string.Format("{0}:u", tenancy2.LastActionDate));

        }
    }
}
