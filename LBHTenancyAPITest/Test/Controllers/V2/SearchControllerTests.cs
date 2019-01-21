using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBH.Data.Domain;
using LBHTenancyAPI.Controllers.V2;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.UseCases.V2.Search;
using LBHTenancyAPI.UseCases.V2.Search.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers.V2
{
    public class SearchControllerTests
    {
        private SearchController _classUnderTest;
        private Mock<ISearchTenancyUseCase> _mock;

        public SearchControllerTests()
        {
            _mock = new Mock<ISearchTenancyUseCase>();
            _classUnderTest = new SearchController(_mock.Object);
        }

        [Fact]
        public async Task GivenValidSearchTenancyRequest_WhenCallingGet_ThenShouldReturn200()
        {
            //arrange
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<SearchTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchTenancyResponse
                {
                    Tenancies = new List<SearchTenancySummary>
                    {

                    }
                });

            var request = new SearchTenancyRequest
            {
                TenancyRef = "test"
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            objectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GivenValidSearchTenancyRequest_WhenCallingGet_ThenShouldReturnAPIResponseListOfTenancySearch()
        {
            //arrange
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<SearchTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchTenancyResponse
                {
                    Tenancies = new List<SearchTenancySummary>
                    {

                    }
                });

            var request = new SearchTenancyRequest
            {
                TenancyRef = "test"
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            var getContacts = objectResult?.Value as APIResponse<SearchTenancyResponse>;
            getContacts.Should().NotBeNull();
        }

        [Theory]
        [InlineData("fName", "lName")]
        [InlineData("fName1", "lName2")]
        public async Task GivenValidGetContactsForTenancyRequest_WhenCallingGet_ThenShouldReturnListOfContactsForTenancy_AndThePropertiesShouldBeMappedCorrectly(
            string firstName, string lastName)
        {
            //arrange
            var primaryContactName = $"{firstName} {lastName}";
            var postcode = "EC12 1DS";
            var tenancyRef = "tenRef";
            decimal currentBalance = (decimal)100.12;
            var propertyRef = "propRef";
            var tenure = "tenure";
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<SearchTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchTenancyResponse
                {
                    Tenancies = new List<SearchTenancySummary>
                    {
                        new SearchTenancySummary
                        {
                            PrimaryContact = new PrimaryContact
                            {
                                Name = primaryContactName,
                                Postcode = postcode
                            },
                            TenancyRef = tenancyRef,
                            CurrentBalance = new Currency(currentBalance),
                            PropertyRef = propertyRef,
                            Tenure = tenure
                        }
                    }
                });

            var request = new SearchTenancyRequest
            {
                FirstName = firstName,
                LastName = lastName
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            var getContacts = objectResult?.Value as APIResponse<SearchTenancyResponse>;
            getContacts.Should().NotBeNull();
            getContacts.Data.Tenancies.Should().NotBeNullOrEmpty();
            getContacts.Data.Tenancies[0].PrimaryContact.Name.Should().BeEquivalentTo(primaryContactName);
            getContacts.Data.Tenancies[0].PrimaryContact.Postcode.Should().BeEquivalentTo(postcode);
            getContacts.Data.Tenancies[0].TenancyRef.Should().BeEquivalentTo(tenancyRef);
            getContacts.Data.Tenancies[0].CurrentBalance.Should().NotBeNull();
            getContacts.Data.Tenancies[0].CurrentBalance.Value.Should().Be(currentBalance);
            getContacts.Data.Tenancies[0].CurrentBalance.CurrencyCode.Should().BeEquivalentTo("GBP");
            getContacts.Data.Tenancies[0].PropertyRef.Should().BeEquivalentTo(propertyRef);
            getContacts.Data.Tenancies[0].Tenure.Should().BeEquivalentTo(tenure);
        }
    }
}
