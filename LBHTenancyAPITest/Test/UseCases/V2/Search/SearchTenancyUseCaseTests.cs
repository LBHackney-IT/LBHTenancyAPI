using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V2.Search;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;
using LBHTenancyAPI.UseCases.V2.Search;
using LBHTenancyAPI.UseCases.V2.Search.Models;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LBHTenancyAPITest.Test.UseCases.V2.Search
{
    public class SearchTenancyUseCaseTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ISearchTenancyUseCase _classUnderTest;
        private readonly Mock<ISearchGateway> _fakeGateway;

        public SearchTenancyUseCaseTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _fakeGateway = new Mock<ISearchGateway>();

            _classUnderTest = new SearchTenancyUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidedInput__WhenExecuteAsync_GatewayReceivesCorrectInput()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.TenancyRef.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new PagedResults<TenancyListItem>());

            var request = new SearchTenancyRequest
            {
                TenancyRef = tenancyAgreementRef
            };
            //act
            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            _fakeGateway.Verify(v => v.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.TenancyRef.Equals("Test")), CancellationToken.None));
        }

        [Fact]
        public async Task GivenNullInput_WhenExecuteAsync_ThenShouldThrowBadRequestException()
        {
            //arrange
            SearchTenancyRequest request = null;
            //act
            //assert
            // ReSharper disable once ExpressionIsAlwaysNull
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
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.TenancyRef.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(null as PagedResults<TenancyListItem>);

            var request = new SearchTenancyRequest
            {
                TenancyRef = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Tenancies.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidedInput__WhenExecuteAsync_ThenShouldReturnListOfTenancySummaries()
        {
            //arrange
            var tenancy1 = new TenancyListItem
            {
                PrimaryContactName = "test",
                TenancyRef = "tRef",
                ArrearsAgreementStartDate = DateTime.Now,
                ArrearsAgreementStatus = "Active",
                CurrentBalance = 1000.12m,
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
                CurrentBalance = 2000.34m,
                LastActionCode = "ACC2",
                LastActionDate = DateTime.Now.AddDays(-2),
                PrimaryContactPostcode = "test2",
                PrimaryContactShortAddress = "123DreryLane2",
                PropertyRef = "22",
                Tenure = "LongLease2"
            };
            var tenancy3 = new TenancyListItem
            {
                PrimaryContactName = "test3",
                TenancyRef = "tRef2",
                ArrearsAgreementStartDate = DateTime.Now,
                ArrearsAgreementStatus = "Active2",
                CurrentBalance = 2000.34m,
                LastActionCode = "ACC2",
                LastActionDate = DateTime.Now.AddDays(-2),
                PrimaryContactPostcode = "test2",
                PrimaryContactShortAddress = "123DreryLane2",
                PropertyRef = "22",
                Tenure = "LongLease2"
            };
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.TenancyRef.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new PagedResults<TenancyListItem>
                {
                    Results = new List<TenancyListItem>
                    {
                        tenancy1,
                        tenancy2,
                        tenancy3
                    }
                });

            var request = new SearchTenancyRequest
            {
                TenancyRef = tenancyAgreementRef
            };
            //act T
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Tenancies.Should().NotBeNullOrEmpty();
            response.Tenancies[0].PropertyRef.Should().BeEquivalentTo(tenancy1.PropertyRef);
            response.Tenancies[0].TenancyRef.Should().BeEquivalentTo(tenancy1.TenancyRef);
            response.Tenancies[0].Tenure.Should().BeEquivalentTo(tenancy1.Tenure);

            response.Tenancies[0].CurrentBalance.Should().NotBeNull();
            response.Tenancies[0].CurrentBalance.Value.Should().Be(tenancy1.CurrentBalance);
            response.Tenancies[0].CurrentBalance.CurrencyCode.Should().BeEquivalentTo("GBP");

            response.Tenancies[0].PrimaryContact.Name.Should().BeEquivalentTo(tenancy1.PrimaryContactName);
            response.Tenancies[0].PrimaryContact.Postcode.Should().BeEquivalentTo(tenancy1.PrimaryContactPostcode);
            response.Tenancies[0].PrimaryContact.ShortAddress.Should().BeEquivalentTo(tenancy1.PrimaryContactShortAddress);

            response.Tenancies[1].PropertyRef.Should().BeEquivalentTo(tenancy2.PropertyRef);
            response.Tenancies[1].TenancyRef.Should().BeEquivalentTo(tenancy2.TenancyRef);
            response.Tenancies[1].Tenure.Should().BeEquivalentTo(tenancy2.Tenure);

            response.Tenancies[1].CurrentBalance.Should().NotBeNull();
            response.Tenancies[1].CurrentBalance.Value.Should().Be(tenancy2.CurrentBalance);
            response.Tenancies[1].CurrentBalance.CurrencyCode.Should().BeEquivalentTo("GBP");

            Console.WriteLine("rebecaaaaaaaaaaa!");

            response.Tenancies[1].PrimaryContact.Name.Should().BeEquivalentTo(tenancy2.PrimaryContactName + "&" + tenancy3.PrimaryContactName);
            response.Tenancies[1].PrimaryContact.Postcode.Should().BeEquivalentTo(tenancy2.PrimaryContactPostcode);
            response.Tenancies[1].PrimaryContact.ShortAddress.Should().BeEquivalentTo(tenancy2.PrimaryContactShortAddress);
        }

        [Theory]
        [InlineData(11,10,2)]
        [InlineData(10, 10, 1)]
        [InlineData(0, 10, 1)]
        [InlineData(1, 10, 1)]
        [InlineData(679, 10, 68)]
        public async Task GivenValidedInput_WhenGatewayRespondsTotalCount_ThenPageCountShouldBe(int totalCount, int pageSize, int expectedPageCount)
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.TenancyRef.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new PagedResults<TenancyListItem>
                {
                    TotalResultsCount = totalCount,
                });

            var request = new SearchTenancyRequest
            {
                TenancyRef = tenancyAgreementRef,
                PageSize = pageSize
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.PageCount.Should().Be(expectedPageCount);
        }

        [Fact]
        public async Task GivenValidedInput_WhenGatewayRespondsWithNullArrearsAgreement_ThenArrearsAgreementShouldBeNull()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            var results = new PagedResults<TenancyListItem>
            {
                Results = new List<TenancyListItem>
                {
                    new TenancyListItem
                    {
                        ArrearsAgreementStatus = "",
                        ArrearsAgreementStartDate = DateTime.MinValue
                    }
                },
                TotalResultsCount = 1,
            };
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.TenancyRef.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(results);

            var request = new SearchTenancyRequest
            {
                TenancyRef = tenancyAgreementRef,
                PageSize = 10,
                Page = 0
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Tenancies[0].Should().NotBeNull();
        }
    }
}
