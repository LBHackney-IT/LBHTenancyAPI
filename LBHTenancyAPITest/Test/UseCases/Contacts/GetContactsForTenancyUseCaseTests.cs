using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.Contacts;
using LBHTenancyAPI.Infrastructure.Exceptions;
using LBHTenancyAPI.UseCases.Contacts;
using LBHTenancyAPI.UseCases.Contacts.Models;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.Contacts
{

    public class GetContactsForTenancyUseCaseTests
    {
        private readonly IGetContactsForTenancyUseCase _classUnderTest;
        private readonly Mock<IContactsGateway> _fakeGateway;

        public GetContactsForTenancyUseCaseTests()
        {
            _fakeGateway = new Mock<IContactsGateway>();
            
            _classUnderTest = new GetContactsForTenancyUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidedInput__WhenExecuteAsync_GatewayReceivesCorrectInput()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.GetContactsByTenancyReferenceAsync(It.Is<GetContactsForTenancyRequest>(i => i.TenancyAgreementReference.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new List<LBH.Data.Domain.Contact>
                {

                });

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            _fakeGateway.Verify(v => v.GetContactsByTenancyReferenceAsync(It.Is<GetContactsForTenancyRequest>(i => i.TenancyAgreementReference.Equals("Test")), CancellationToken.None));
        }

        [Fact]
        public async Task GivenNullInput_WhenExecuteAsync_ThenShouldThrowBadRequestException()
        {
            //arrange
            GetContactsForTenancyRequest request = null;
            //act
            //assert
            await Assert.ThrowsAsync<BadRequestException>(async ()=> await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task Given_InvalidInput_ThenShouldThrowBadRequestException()
        {
            //arrange
            var request = new GetContactsForTenancyRequest();
            //act
            //assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task GivenValidedInput_WhenGatewayRespondsWithNull_ThenContactsListShouldBeNull()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.GetContactsByTenancyReferenceAsync(It.Is<GetContactsForTenancyRequest>(i => i.TenancyAgreementReference.Equals("Test")), CancellationToken.None))
                .ReturnsAsync((List<Contact>)null);

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Contacts.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidedInput__WhenExecuteAsync_ThenShouldReturnListOfContacts()
        {
            //arrange
            var contact1 = new Contact
            {
                ContactId = Guid.NewGuid(),
                EmailAddress = "test@test.com",
                UniquePropertyReferenceNumber = "",
                AddressLine1 = "Add1",
                AddressLine2 = "Add2",
                AddressLine3 = "Add3",
            };
            var contact2 = new Contact
            {

            };
            var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.GetContactsByTenancyReferenceAsync(It.Is<GetContactsForTenancyRequest>(i => i.TenancyAgreementReference.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new List<LBH.Data.Domain.Contact>
                {
                    contact1,
                    contact2
                });

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Contacts.Should().NotBeNullOrEmpty();
            response.Contacts[0].Should().BeEquivalentTo(contact1);
            response.Contacts[1].Should().BeEquivalentTo(contact2);
        }
    }
}
