using System;
using LBHTenancyAPI.Factories;
using Xunit;
using FluentAssertions;
using AgreementService;

namespace LBHTenancyAPITest.Test.Factories
{
    public class WCFClientFactoryTests
    {
        private IWCFClientFactory _classUnderTest;

        [Fact]
        public void GivenInValidDetails_ShouldThrowUriFormatException()
        {
            //arrange
            _classUnderTest = new WCFClientFactory();
            //act
            //assert
            Assert.Throws<UriFormatException>(()=> _classUnderTest.CreateClient<IArrearsAgreementService>("test"));
        }

        [Fact]
        public void GivenValidDetails_ShouldReturnNewClient()
        {
            //arrange
            _classUnderTest = new WCFClientFactory();
            //act
            var client = _classUnderTest.CreateClient<IArrearsAgreementService>("http://www.test.com/test.svc");
            //assert
            client.Should().NotBeNull();
        }
    }
}
