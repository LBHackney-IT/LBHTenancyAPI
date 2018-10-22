using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.Contacts;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client.Factory;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Exceptions;
using LBHTenancyAPI.UseCases.V1.Contacts.Models;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.V1.Contacts
{
    public class Dynamics365ApiContactsGatewayTests
    {
        private IContactsGateway _classUnderTest;

        [Theory]
        [InlineData(
            "e7150cbd-844b-45ff-a8fc-475ef62887ff", "test@email.com", "t", "add1", "add2", "add3", "fName", "lName", "fName",
            "larn", "tel1", "tel2", "tel3", true, true, "hRef", "title", "fullAddDisp",
            "fullAddSearch", "postcode", "2017-09-08T19:01:55.714942+03:00Z", "homeId", 2, true)]
        [InlineData(
            "e7150cbd-844b-45ff-a8fc-475ef62887fe", "test@email.com2", "t2", "add12", "add22", "add32", "fName2", "lName2", "fName2",
            "larn2", "tel12", "tel22", "tel32", true, true, "hRef2", "title2", "fullAddDisp2",
            "fullAddSearch2", "postcode2", "2018-09-08T19:01:55.714942+03:00Z", "homeId2", 2, true)]
        public async Task GivenValidInput_WhenCallingGetContactsByTenancyReferenceAsync_ThenShouldReturnContacts(
            string contactId, string emailAddress, string uprn, string add1, string add2, string add3, string firstName, string lastName, string fullName,
            string larn, string tel1, string tel2, string tel3, bool cautionAlert, bool propCautionAlert, string houseRef, string title, string fullAddDisp,
            string fullAddSearch, string postCode, string dateOfBirth, string hackneyHomesId, int age, bool responsible)
        {
            //arrange
            var mockClientFactory = new Mock<IDynamics365ClientFactory>();
            var mockHttpClient = new Mock<IHttpClient>();
            mockClientFactory.Setup(s => s.CreateClientAsync(true)).ReturnsAsync(mockHttpClient.Object);

            var json =
                $@"{{
                        'value':
                        [
                            {{
                                '@odata.etag': 'W\/\'{010101}\'',
                                'accountid': '{Guid.NewGuid().ToString()}',
                                'contact1_x002e_telephone3': '{tel3}',
                                'contact1_x002e_telephone2': '{tel2}',
                                'contact1_x002e_hackney_cautionaryalert@OData.Community.Display.V1.FormattedValue': 'No',
                                'contact1_x002e_hackney_cautionaryalert': {cautionAlert.ToString().ToLower()},
                                'contact1_x002e_hackney_larn': '{larn}',
                                'contact1_x002e_hackney_title': '{title}',
                                'contact1_x002e_hackney_age': '{age}',
                                'contact1_x002e_contactid': '{contactId}',
                                'contact1_x002e_firstname': '{firstName}',
                                'contact1_x002e_hackney_propertycautionaryalert@OData.Community.Display.V1.FormattedValue': 'No',
                                'contact1_x002e_hackney_propertycautionaryalert': {propCautionAlert.ToString().ToLower()},
                                'contact1_x002e_address1_postalcode': '{postCode}',
                                'contact1_x002e_address1_composite': '{fullAddDisp}',
                                'contact1_x002e_hackney_hackneyhomesid': '{hackneyHomesId}',
                                'contact1_x002e_housing_house_ref': '{houseRef}',
                                'contact1_x002e_hackney_uprn': '{uprn}',
                                'contact1_x002e_birthdate@OData.Community.Display.V1.FormattedValue': '15\/05\/1971',
                                'contact1_x002e_birthdate': '{dateOfBirth}',
                                'contact1_x002e_address1_name': '{fullAddSearch}',
                                'contact1_x002e_lastname': '{lastName}',
                                'contact1_x002e_fullname': '{fullName}',
                                'contact1_x002e_address1_line3': '{add3}',
                                'contact1_x002e_address1_line2': '{add2}',
                                'contact1_x002e_address1_line1': '{add1}',
                                'contact1_x002e_telephone1': '{tel1}',
                                'contact1_x002e_emailAddress1': '{emailAddress}',
                                'contact1_x002e_hackney_responsible': '{responsible}'
                            }}
                        ]
                    }}";

            mockHttpClient.Setup(s => s.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            _classUnderTest = new Dynamics365RestApiContactsGateway(mockClientFactory.Object);

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = "Test"
            };
            //act
            var response = await _classUnderTest.GetContactsByTenancyReferenceAsync(request, CancellationToken.None).ConfigureAwait(false);
            //assert
            response.Should().NotBeNullOrEmpty();

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
                Responsible = responsible
            };
            response[0].Should().BeEquivalentTo(contact);
        }

        [Fact]
        public async Task GivenValidInput_WhenCallingGetContactsByTenancyReferenceAsync_AndDynamics365ReturnsError_ThenShouldThrowException()
        {
            //arrange
            var mockClientFactory = new Mock<IDynamics365ClientFactory>();
            var mockHttpClient = new Mock<IHttpClient>();
            mockClientFactory.Setup(s => s.CreateClientAsync(true)).ReturnsAsync(mockHttpClient.Object);

            mockHttpClient.Setup(s => s.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

            _classUnderTest = new Dynamics365RestApiContactsGateway(mockClientFactory.Object);

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = "Test"
            };
            //act
            //assert
            await Assert.ThrowsAsync<Dynamics365RestApiException>(async ()=> await _classUnderTest.GetContactsByTenancyReferenceAsync(request, CancellationToken.None).ConfigureAwait(false));
        }

        [Fact]
        public async Task GivenValidInput_WhenCallingGetContactsByTenancyReferenceAsync_AndDynamics365ReturnsNoJson_ThenShouldReturnnull()
        {
            //arrange
            var mockClientFactory = new Mock<IDynamics365ClientFactory>();
            var mockHttpClient = new Mock<IHttpClient>();
            mockClientFactory.Setup(s => s.CreateClientAsync(true)).ReturnsAsync(mockHttpClient.Object);

            var json = string.Empty;

            mockHttpClient.Setup(s => s.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            _classUnderTest = new Dynamics365RestApiContactsGateway(mockClientFactory.Object);

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = "Test"
            };
            //act
            var response = await _classUnderTest.GetContactsByTenancyReferenceAsync(request, CancellationToken.None).ConfigureAwait(false);
            //assert
            response.Should().BeNullOrEmpty();
        }
    }
}
