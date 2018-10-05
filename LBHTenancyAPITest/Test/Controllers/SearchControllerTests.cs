using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBH.Data.Domain;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.UseCases.Contacts;
using LBHTenancyAPI.UseCases.Contacts.Models;
using LBHTenancyAPI.UseCases.Search;
using LBHTenancyAPI.UseCases.Search.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
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
        public async Task GivenValidSearchTenancyRequest_WhenCallingGet_THenShouldReturn200()
        {
            //arrange
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<SearchTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchTenancyResponse
                {
                    Tenancies = new List<SearchSummary>
                    {

                    }
                });

            var request = new SearchTenancyRequest
            {
                SearchTerm = "test"
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
                    Tenancies = new List<SearchSummary>
                    {

                    }
                });

            var request = new SearchTenancyRequest
            {
                SearchTerm = "test"
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
            var tenancySummary = new TenancyListItem
            {
                PrimaryContactName = $"{firstName} {lastName}"
            };
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<SearchTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchTenancyResponse
                {
                    Tenancies = new List<SearchSummary>
                    {
                        new SearchSummary
                        {
                            PrimaryContact = new PrimaryContact
                            {
                                Name = $"{firstName} {lastName}"
                            }
                        }
                    }
                });

            var request = new SearchTenancyRequest
            {
                SearchTerm = "test"
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
            getContacts.Data.Tenancies.Should().BeEquivalentTo(tenancySummary);
        }
    }
}
