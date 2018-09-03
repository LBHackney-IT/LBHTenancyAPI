using System;
using System.Threading.Tasks;
using FluentAssertions;
using UHAuthenticationService;
using Xunit;

namespace LBHWebservices.AuthenticationService.Tests.Integration
{
    public class LoginTest
    {
        private IAuthenticationService _classUnderTest;
        public LoginTest()
        {
            _classUnderTest = new AuthenticationServiceClient();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "")]
        public async Task GivenValidLoginCredentials_LoginWithActiveDir_ShouldReturnSuccess(string username, string password)
        {
            //arrange
            var request = new LoginRequest
            {
                authenticationType = SecurityAuthenticationType.ActiveDir,
                username = username,
                password = password
            };
            //act
            var response = await _classUnderTest.LoginAsync(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.LoginResult.LogonSuccess.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "")]
        public async Task GivenValidLoginCredentials_LoginWithAuto_ShouldReturnSuccess(string username, string password)
        {
            //arrange
            var request = new LoginRequest
            {
                authenticationType = SecurityAuthenticationType.Auto,
                username = username,
                password = password
            };
            //act
            var response = await _classUnderTest.LoginAsync(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.LoginResult.LogonSuccess.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "")]
        public async Task GivenValidLoginCredentials_LoginWithBasic_ShouldReturnSuccess(string username, string password)
        {
            //arrange
            var request = new LoginRequest
            {
                authenticationType = SecurityAuthenticationType.Basic,
                username = username,
                password = password
            };
            //act
            var response = await _classUnderTest.LoginAsync(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.LoginResult.LogonSuccess.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "")]
        public async Task GivenValidLoginCredentials_LoginWithUhWeb_ShouldReturnSuccess(string username, string password)
        {
            //arrange
            var request = new LoginRequest
            {
                authenticationType = SecurityAuthenticationType.Uhweb,
                username = username,
                password = password
            };
            //act
            var response = await _classUnderTest.LoginAsync(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.LoginResult.LogonSuccess.Should().BeTrue();
        }


        [Theory]
        [InlineData("", "")]
        [InlineData("", "")]
        public async Task GivenValidLoginCredentials_LoginWithUHT_ShouldReturnSuccess(string username, string password)
        {
            //arrange
            var request = new LoginRequest
            {
                authenticationType = SecurityAuthenticationType.Uht,
                username = username,
                password = password
            };
            //act
            var response = await _classUnderTest.LoginAsync(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.LoginResult.LogonSuccess.Should().BeTrue();
        }
    }
}
