using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBH.Data.Domain;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Controllers.V1;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.UseCases.V1.Contacts;
using LBHTenancyAPI.UseCases.V1.Contacts.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers.V1
{
    public class ContactsControllerTests
    {
        private ContactController _classUnderTest;
        private Mock<IGetContactsForTenancyUseCase> _mock;

        public ContactsControllerTests()
        {
            _mock = new Mock<IGetContactsForTenancyUseCase>();
            _classUnderTest = new ContactController(_mock.Object);
        }

        [Fact]
        public async Task GivenValidGetContactsForTenancyRequest_WhenCallingGet_THenShouldReturn200()
        {
            //arrange
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<GetContactsForTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetContactsForTenancyResponse
                {
                    Contacts = new List<ContactsForTenancy>
                    {

                    }
                });

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = "test"
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
        public async Task GivenValidGetContactsForTenancyRequest_WhenCallingGet_ThenShouldReturnAPIResponseListOfContactsForTenancy()
        {
            //arrange
            var contact = new Contact
            {
                AddressLine2 = "test"
            };
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<GetContactsForTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetContactsForTenancyResponse
                {
                    Contacts = new List<ContactsForTenancy>
                    {
                        new ContactsForTenancy(contact)
                    }
                });

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = "test"
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            var getContacts = objectResult?.Value as APIResponse<GetContactsForTenancyResponse>;
            getContacts.Should().NotBeNull();
        }

        [Theory]
        [InlineData(
            "e7150cbd-844b-45ff-a8fc-475ef62887ff", "test@email.com", "t", "add1", "add2", "add3", "fName","lName", "fName",
            "larn","tel1", "tel2", "tel3", true, true, "hRef", "title", "fullAddDisp",
            "fullAddSearch", "postcode", "2017-09-08T19:01:55.714942+03:00Z", "homeId", 2)]
        [InlineData(
            "e7150cbd-844b-45ff-a8fc-475ef62887fe", "test@email.com2", "t2", "add12", "add22", "add32", "fName2", "lName2", "fName2",
            "larn2", "tel12", "tel22", "tel32", true, true, "hRef2", "title2", "fullAddDisp2",
            "fullAddSearch2", "postcode2", "2018-09-08T19:01:55.714942+03:00Z", "homeId2", 2)]
        public async Task GivenValidGetContactsForTenancyRequest_WhenCallingGet_ThenShouldReturnListOfContactsForTenancy_AndThePropertiesShouldBeMappedCorrectly(
            string contactId, string emailAddress, string uprn, string add1, string add2, string add3, string firstName, string lastName, string fullName,
            string larn, string tel1, string tel2, string tel3, bool cautionAlert, bool propCautionAlert, string houseRef, string title, string fullAddDisp,
            string fullAddSearch, string postCode, string dateOfBirth, string hackneyHomesId, int age)
        {
            //arrange
            var contact = new Contact
            {
                ContactId = new Guid(contactId),
                EmailAddress = emailAddress,
                UniquePropertyReferenceNumber = uprn,
                AddressLine1 = add1,
                AddressLine2 = add2,
                AddressLine3 = add3,
                Firstname = firstName,
                LastName = lastName,
                FullName = fullName,
                Larn = larn,
                Telephone1 = tel1,
                Telephone2 = tel2,
                Telephone3 = tel3,
                CautionaryAlert = cautionAlert,
                PropertyCautionaryAlert = propCautionAlert,
                HouseRef = houseRef,
                Title = title,
                FullAddressDisplay = fullAddDisp,
                FullAddressSearch = fullAddSearch,
                PostCode = postCode,
                DateOfBirth = DateTime.Parse(dateOfBirth),
                HackneyHomesId = hackneyHomesId,
                Age = age,
            };
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<GetContactsForTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetContactsForTenancyResponse
                {
                    Contacts = new List<ContactsForTenancy>
                    {
                        new ContactsForTenancy(contact)
                    }
                });

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = "test"
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            var getContacts = objectResult?.Value as APIResponse<GetContactsForTenancyResponse>;
            getContacts.Should().NotBeNull();
            getContacts.Data.Contacts.Should().NotBeNullOrEmpty();
            getContacts.Data.Contacts[0].Should().BeEquivalentTo(contact);
        }
    }
}
