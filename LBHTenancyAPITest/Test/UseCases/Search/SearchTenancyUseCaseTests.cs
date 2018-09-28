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

                } as IList<LBH.Data.Domain.TenancyListItem>);

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
                .ReturnsAsync(null as IList<LBH.Data.Domain.TenancyListItem>);

            var request = new SearchTenancyRequest
            {
                SearchTerm = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.SearchResults.Should().BeNull();
        }

        //[Fact]
        //public async Task GivenValidedInput__WhenExecuteAsync_ThenShouldReturnListOfContacts()
        //{
        //    //arrange
        //    var contact1 = new Contact
        //    {
        //        ContactId = Guid.NewGuid(),
        //        EmailAddress = "test@test.com",
        //        UniquePropertyReferenceNumber = "",
        //        AddressLine1 = "Add1",
        //        AddressLine2 = "Add2",
        //        AddressLine3 = "Add3",
        //    };
        //    var contact2 = new Contact
        //    {

        //    };
        //    var tenancyAgreementRef = "Test";
        //    _fakeGateway.Setup(s => s.GetContactsByTenancyReferenceAsync(It.Is<GetContactsForTenancyRequest>(i => i.TenancyAgreementReference.Equals("Test")), CancellationToken.None))
        //        .ReturnsAsync(new List<LBH.Data.Domain.Contact>
        //        {
        //            contact1,
        //            contact2
        //        });

        //    var request = new GetContactsForTenancyRequest
        //    {
        //        TenancyAgreementReference = tenancyAgreementRef
        //    };
        //    //act
        //    var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
        //    //assert
        //    response.Should().NotBeNull();
        //    response.Contacts.Should().NotBeNullOrEmpty();
        //    response.Contacts[0].Should().BeEquivalentTo(contact1);
        //    response.Contacts[1].Should().BeEquivalentTo(contact2);
        //}
    }
}
