using System;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.Dynamics365.Authentication;
using LBHTenancyAPI.Settings.CRM;
using Xunit;
using FluentAssertions;
using LBHTenancyAPI.Infrastructure.Dynamics365.Authentication.Exceptions;

namespace LBHTenancyAPITest.Test.Infrastructure
{
    [Trait("Category","Configuration")]
    public class Dynamics365AuthenticationServiceTests
    {
        private IDynamics365AuthenticationService _classUnderTest;

        [Fact(Skip = "Need Environment Variables to be set, this is an integration test")]
        public async Task Given_CorrectCredentials_When_CallingGetAccessTokenAsync_Should_ReturnValidToken()
        {
            //arrange
            _classUnderTest = new Dynamics365AuthenticationService(new Dynamics365Settings
            {
                AppKey = Environment.GetEnvironmentVariable("Dynamics365Settings__AppKey"),
                ClientId = Environment.GetEnvironmentVariable("Dynamics365Settings__ClientId"),
                OrganizationUrl = Environment.GetEnvironmentVariable("Dynamics365Settings__OrganizationUrl"),
                TenantId = Environment.GetEnvironmentVariable("Dynamics365Settings__TenantId"),
                AadInstance = Environment.GetEnvironmentVariable("Dynamics365Settings__AadInstance"),
            });
            //act
            var response = await _classUnderTest.GetAccessTokenAsync().ConfigureAwait(false);
            //assert
            response.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Given_IncorrectCredentials_When_CallingGetAccessTokenAsync_Should_ThrowException()
        {
            //arrange
            _classUnderTest = new Dynamics365AuthenticationService(new Dynamics365Settings
            {
                AppKey = "",
                ClientId = Environment.GetEnvironmentVariable("Dynamics365Settings__ClientId"),
                OrganizationUrl = Environment.GetEnvironmentVariable("Dynamics365Settings__OrganizationUrl"),
                TenantId = Environment.GetEnvironmentVariable("Dynamics365Settings__TenantId"),
                AadInstance = Environment.GetEnvironmentVariable("Dynamics365Settings__AadInstance"),
            });
            //act
            //assert
            await Assert.ThrowsAsync<Dynamics365IsNotConfiguredException>(async () => await _classUnderTest.GetAccessTokenAsync().ConfigureAwait(false));
        }

        [Fact]
        public async Task GivenNullSettings_When_CallingGetAccessTokenAsync_Should_ThrowException()
        {
            //arrange
            _classUnderTest = new Dynamics365AuthenticationService(null);
            //act
            //assert
            await Assert.ThrowsAsync<Dynamics365IsNotConfiguredException>(async () => await _classUnderTest.GetAccessTokenAsync().ConfigureAwait(false));
        }
    }
}
