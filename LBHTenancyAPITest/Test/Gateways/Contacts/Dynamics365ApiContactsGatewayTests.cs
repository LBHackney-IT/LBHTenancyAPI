using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.Gateways.Contacts;
using LBHTenancyAPI.Settings.CRM;
using LBHTenancyAPI.UseCases.Contacts.Models;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.Contacts
{
    public class Dynamics365ApiContactsGatewayTests
    {
        private IContactsGateway _classUnderTest;

        [Theory]
        [InlineData("")]
        [InlineData("")]
        public async Task GivenValidInput_WhenCallingGetContactsByTenancyReferenceAsync_ThenShouldReturnContacts(string contactIdGuid)
        {
            //arrange
            var mockClientFactory = new Mock<IDynamics365ClientFactory>();
            var mockHttpClient = new Mock<IHttpClient>();
            mockClientFactory.Setup(s => s.CreateClientAsync()).ReturnsAsync(mockHttpClient.Object);

            var json =
                $@"{{
                        'values':
                        [
                            {{
                                'contactid' : {new Guid(contactIdGuid).ToString()}
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
            response[0].ContactId.Should().Be(new Guid(contactIdGuid));
        }

        [Fact]
        public async Task GivenValidInput_WhenCallingGetContactsByTenancyReferenceAsync_AndDynamics365ReturnsError_ThenShouldThrowException()
        {
            //arrange
            var mockClientFactory = new Mock<IDynamics365ClientFactory>();
            var mockHttpClient = new Mock<IHttpClient>();
            mockClientFactory.Setup(s => s.CreateClientAsync()).ReturnsAsync(mockHttpClient.Object);

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
            mockClientFactory.Setup(s => s.CreateClientAsync()).ReturnsAsync(mockHttpClient.Object);

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
